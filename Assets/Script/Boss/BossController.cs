using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHP = 10;
    public int currentHP;

    [Header("��� �ð�")]
    public float WaitTime;

    [Header("���� ������Ʈ")]
    public Transform player;
    public Transform firePoint;

    [Header("����")]
    public List<BossPattern> normalPatterns = new List<BossPattern>();
    public BossPattern specialPattern;

    [Header("�Ѿ� Ǯ��")]
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

            pattern.Initialize(this);  // BossController ���� ����
        }

        if (bulletPool == null)
            Debug.LogWarning("BulletPool�� �Ҵ���� �ʾҽ��ϴ�!");

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
                    print(normalPatterns[idx] + " �����");
                    yield return StartCoroutine(normalPatterns[idx].ExecutePattern(currentHP, maxHP));
                }
            }
            patternCount++;
            yield return new WaitForSeconds(WaitTime);
        }

        Debug.Log("Boss Dead!");
    }
}
