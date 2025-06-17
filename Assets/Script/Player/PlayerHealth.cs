using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [Header("Player Models by Health (Index 0 = Dead)")]
    public GameObject[] healthModels; // Index 0: 체력 0(죽은 상태), 1: 체력 1, ..., maxHealth: 체력 max
    GameObject visualObject;

    private Animator animator;
    private PlayerMovement movement;
    private Collider playerCollider;

    private bool isInvincible = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider>();
        visualObject = healthModels[healthModels.Length - 1];
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateModel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateModel();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine(0.3f, 0.1f));
        }
    }

    private IEnumerator InvincibilityCoroutine(float duration, float blinkInterval)
    {
        isInvincible = true;

        if (playerCollider != null)
            playerCollider.enabled = false;

        float timer = 0f;
        bool visible = true;

        while (timer < duration)
        {
            visible = !visible;
            if (visualObject != null)
                visualObject.SetActive(visible);

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (visualObject != null)
            visualObject.SetActive(true);

        if (playerCollider != null)
            playerCollider.enabled = true;

        isInvincible = false;
    }

    void UpdateModel()
    {
             visualObject.SetActive(false);
             visualObject = healthModels[currentHealth];
             visualObject.SetActive(true);
    }

    void Die()
    {
        if (animator != null)
            animator.SetTrigger("Dead");

        if (movement != null)
            movement.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;

        Debug.Log("플레이어 사망");
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamage(1);
        }
    }
}
