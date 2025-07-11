using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public BulletPool bulletPool;
    public BossEffectController effectController;

    [Header("Patterns")]
    public List<BossPattern> patterns;
    public BossPattern specialPattern;

    [Header("Settings")]
    public float patternCooldown = 1f;
    public List<WarpPoint> warpPoints;

    private int currentPatternIndex = 0;
    private Coroutine patternCoroutine;
    private Coroutine specialCoroutine;
    private bool isHit = false;
    private bool isInSpecialPattern = false;

    [SerializeField] private int maxhp = 3;
    [SerializeField] private int currenthp = 0;

    private void Awake()
    {
        foreach (var pattern in patterns) pattern.Initialize(this); 
        specialPattern.Initialize(this);
    }

    private void Start()
    {
        currenthp = maxhp;
        patternCoroutine = StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            BossPattern pattern = patterns[Random.Range(0, patterns.Count)];
            yield return StartCoroutine(pattern.ExecutePattern(currenthp, maxhp));

            currentPatternIndex++;
            if (currentPatternIndex >= patterns.Count)
            {
                currentPatternIndex = 0;

                // 3패턴마다 특수 패턴 시도
                if (!isInSpecialPattern)
                {
                    StartSpecialPattern();
                    yield break; // 패턴 루프 중단, 특수 패턴으로 전환
                }
            }

            yield return new WaitForSeconds(patternCooldown);
        }
    }

    public void StartSpecialPattern()
    {
        if (specialCoroutine != null) return;

        isInSpecialPattern = true;
        isHit = false;

        SetWarpPointsActive(true);
        specialCoroutine = StartCoroutine(specialPattern.ExecutePattern(currenthp, maxhp));
    }

    public void EndSpecialPattern()
    {
        if (!isInSpecialPattern) return;

        isInSpecialPattern = false;
        SetWarpPointsActive(false);

        if (specialCoroutine != null)
        {
            StopCoroutine(specialCoroutine);
            specialCoroutine = null;
        }

        patternCoroutine = StartCoroutine(PatternLoop());
    }

    public void OnHitByWarp()
    {
        print("피격");
        effectController.PlayHitEffect();
        currenthp--;
        if (!isInSpecialPattern || isHit) return;

        isHit = true;
        //effectController.PlayHitEffect();

        EndSpecialPattern();
    }

    private void SetWarpPointsActive(bool isActive)
    {
        foreach (var warp in warpPoints)
        {
            if (warp != null)
                warp.gameObject.SetActive(isActive);
        }
    }
}
