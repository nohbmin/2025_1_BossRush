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

    private void Awake()
    {
        foreach (var pattern in patterns) pattern.Initialize(this); 
        specialPattern.Initialize(this);
    }

    private void Start()
    {
        patternCoroutine = StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (true)
        {
            BossPattern pattern = patterns[currentPatternIndex];
            yield return StartCoroutine(pattern.ExecutePattern(GetHP(), GetMaxHP()));

            currentPatternIndex++;
            if (currentPatternIndex >= patterns.Count)
            {
                currentPatternIndex = 0;

                // 3���ϸ��� Ư�� ���� �õ�
                if (!isInSpecialPattern)
                {
                    StartSpecialPattern();
                    yield break; // ���� ���� �ߴ�, Ư�� �������� ��ȯ
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
        specialCoroutine = StartCoroutine(specialPattern.ExecutePattern(GetHP(), GetMaxHP()));
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
        print("�ǰ�");
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

    // ü�� ���� �Լ��� �ʿ��� ������� �����ϰų� ����
    public int GetHP() => 5;
    public int GetMaxHP() => 5;
}
