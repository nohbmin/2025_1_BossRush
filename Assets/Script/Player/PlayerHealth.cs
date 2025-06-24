using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invTime;

    [Header("Player Models by Health (Index 0 = Dead)")]
    public GameObject[] healthModels; // Index 0: ü�� 0(���� ����), 1: ü�� 1, ..., maxHealth: ü�� max
    GameObject visualObject;

    private PlayableDirector playableDirector;
    private PlayerMovement movement;
    private Collider playerCollider;

    private bool isInvincible = false;

    void Awake()
    {
        playableDirector = GetComponentInChildren<PlayableDirector>();
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
        if (currentHealth <= 0 || isInvincible) return; // 무적 상태일 때 데미지를 받지 않도록 수정
    
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateModel();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine(invTime, 0.1f));
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
        if (movement != null)
            movement.isDead = true;

        if (playerCollider != null)
            playerCollider.enabled = false;

        StartCoroutine(Tomain());
    }

    private IEnumerator Tomain()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("DeadScene");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            TakeDamage(1);
        }
    }
}
