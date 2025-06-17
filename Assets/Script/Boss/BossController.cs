using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHP = 10;
    public int currentHP;

    [Header("대기 시간")]
    public float WaitTime;

    [Header("공유 오브젝트")]
    public Transform player;
    public Transform firePoint;

    [Header("패턴")]
    public List<BossPattern> normalPatterns = new List<BossPattern>();
    public BossPattern specialPattern;

    [Header("총알 풀링")]
    public BulletPool bulletPool;

    private int patternCount = 0;

    private void Start()
    {
        currentHP = maxHP;

        var allPatterns = GetComponents<BossPattern>();

        foreach (var pattern in allPatterns)
        {
            if (pattern != specialPattern)
                normalPatterns.Add(pattern);

            pattern.Initialize(this);  // BossController 참조 전달
        }

        if (bulletPool == null)
            Debug.LogWarning("BulletPool이 할당되지 않았습니다!");

        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        while (currentHP > 0)
        {
            if (patternCount > 0 && patternCount % 3 == 0)
            {
                if (specialPattern != null)
                    yield return StartCoroutine(specialPattern.ExecutePattern(currentHP, maxHP));
            }
            else
            {
                if (normalPatterns.Count > 0)
                {
                    int idx = Random.Range(0, normalPatterns.Count);
                    print(normalPatterns[idx] + " 실행됨");
                    yield return StartCoroutine(normalPatterns[idx].ExecutePattern(currentHP, maxHP));
                }
            }
            patternCount++;
            yield return new WaitForSeconds(WaitTime);
        }

        Debug.Log("Boss Dead!");
    }
}
