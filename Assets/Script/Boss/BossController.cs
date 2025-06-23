using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHP = 10;
    [SerializeField]private int currentHP;

    public bool isInSpecialPattern = false;

    [Header("Pattern Settings")]
    public List<BossPattern> normalPatterns;
    public BossPattern specialPattern;
    [SerializeField]private int normalPatternCount = 0;

    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public BulletPool bulletPool;

    [Header("Special Pattern")]
    public float specialPatternDuration = 8f;
    public bool HasBeenHitDuringSpecial = false;

    [Header("Visual Feedback")]
    public Animator animator;
    public BossEffectController effectController; // 쉐이크/임팩트용


    private void Awake()
    {
        foreach (var pattern in normalPatterns)
        {
            pattern.Initialize(this);
        }
        specialPattern.Initialize(this);
    }
    private void Start()
    {
        currentHP = maxHP;
        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (currentHP > 0)
        {
            if (normalPatternCount > 0 && normalPatternCount % 3 == 0)
            {
                yield return StartCoroutine(ExecuteSpecialPattern());
                normalPatternCount = 0;
                continue;
            }

            BossPattern selected = normalPatterns[Random.Range(0, normalPatterns.Count)];
            yield return StartCoroutine(selected.ExecutePattern(currentHP, maxHP));
            normalPatternCount++;

            yield return new WaitForSeconds(1f);
        }

        Die();
    }

    private IEnumerator ExecuteSpecialPattern()
    {
        isInSpecialPattern = true;
        HasBeenHitDuringSpecial = false;

        if (specialPattern != null)
        {
            Coroutine patternRoutine = StartCoroutine(specialPattern.ExecutePattern(currentHP, maxHP));
            float elapsed = 0f;

            while (elapsed < specialPatternDuration && !HasBeenHitDuringSpecial)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            StopCoroutine(patternRoutine);
        }

        isInSpecialPattern = false;
    }

    public void EndSpecialPattern()
    {
        isInSpecialPattern = false;

        // 추가적으로 패턴 루프 재시작이 필요하다면 여기에 작성
        // 예시: StartCoroutine(PatternLoop());
    }

    public void OnHitByWarp()
    {
        if (isInSpecialPattern && !HasBeenHitDuringSpecial)
        {
            HasBeenHitDuringSpecial = true;
            TakeDamage(1);
            effectController?.PlayHitEffect();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            StopAllCoroutines();
            Die();
        }
    }

    private void Die()
    {
        animator?.SetTrigger("Die");
        Debug.Log("Boss Defeated!");
        // 추후 보스 사망 처리 추가
    }
}
