using System.Collections;
using UnityEngine;

public class Boss1_Pattern1 : BossPattern
{
    public float shootDuration = 3f;
    public float fireInterval = 0.25f;
    public float bulletSpeed = 5f;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        // 2초간 플레이어 방향으로 회전
        float waitTime = 2f;
        while (waitTime > 0)
        {
            Vector3 dir = boss.player.position - boss.transform.position;
            dir.z = 0; // XY 평면 기준
            dir.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, dir); // Z축 기준 회전
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRot, Time.deltaTime * 5f);

            waitTime -= Time.deltaTime;
            yield return null;
        }

        // 3초간 0.25초 간격으로 탄환 발사
        float elapsed = 0f;
        while (elapsed < shootDuration)
        {
            Vector3 dir = boss.player.position - boss.firePoint.position;
            dir.z = 0;
            dir.Normalize();

            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir); // XY 기준

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }

            elapsed += fireInterval;
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
