using System.Collections;
using UnityEngine;

public class Boss1_Special : BossPattern
{
    public float fireInterval = 0.3f;
    public float bulletSpeed = 10f;
    public float duration = 10f; // 특수 패턴 지속 시간
    public GameObject[] warpPoints;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        // 보스 상태 설정
        boss.isInSpecialPattern = true;

        // 워프 포인트 활성화
        SetWarpPointsActive(true);

        float elapsed = 0f;

        while (elapsed < duration && boss.isInSpecialPattern)
        {
            // 무작위 방향 회전
            float angle = Random.Range(0f, 360f);
            boss.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // 발사 방향
            Vector3 dir = boss.transform.up;

            // 탄환 생성 및 발사
            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireInterval);
            fireInterval = Mathf.Max(0.05f, fireInterval * 0.95f); // 점차 빨라짐
            elapsed += fireInterval;
        }

        // 종료 처리
        SetWarpPointsActive(false);
        boss.EndSpecialPattern();
    }

    private void SetWarpPointsActive(bool isActive)
    {
        if (warpPoints == null || warpPoints.Length == 0)
        {
            warpPoints = GameObject.FindGameObjectsWithTag("WarpPoint");
        }

        foreach (var wp in warpPoints)
        {
            wp.SetActive(isActive);
        }
    }
}
