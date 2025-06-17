using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Player Models by Health (Index 0 = Dead)")]
    public GameObject[] healthModels; // Index 0: 체력 0(죽은 상태), 1: 체력 1, ..., maxHealth: 체력 max

    private Animator animator;
    private PlayerMovement movement;
    private Collider playerCollider;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider>();
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
    }

    void UpdateModel()
    {
        for (int i = 0; i < healthModels.Length; i++)
        {
            if (healthModels[i] != null)
                healthModels[i].SetActive(i == currentHealth);
        }
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
}
