using System.Collections;
using UnityEngine;

public class Boss1_Pattern2 : BossPattern
{
    public float waitBeforeFire = 1.5f;
    public float fireInterval = 0.5f;
    public float bulletSpeed = 7f;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        // 1.5초 대기
        yield return new WaitForSeconds(waitBeforeFire);

        // 시작 각도 (대각선 방향 중 하나)
        float[] diagonals = { 45f, 135f, 225f, 315f };
        float startAngle = diagonals[Random.Range(0, diagonals.Length)];

        // 시계 or 반시계 방향 (1 또는 -1)
        int direction = Random.value > 0.5f ? 1 : -1;

        for (int i = 0; i < 4; i++)
        {
            float angle = startAngle + direction * i * 90f;

            // XY 평면 기준 회전 (Z축 회전)
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;

            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = rot;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
