using System.Collections;
using UnityEngine;

public class Boss1_Pattern1 : BossPattern
{
    public float shootDuration = 3f;
    public float fireInterval = 0.25f;
    public float bulletSpeed = 5f;
    public float rotationSpeed = 5f;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        // 2초간 플레이어 방향으로 회전
        float waitTime = 2f;
        while (waitTime > 0)
        {
            RotateTowardsPlayer();
            waitTime -= Time.deltaTime;
            yield return null;
        }

        // 3초간 탄 발사 + 지속 회전
        float elapsed = 0f;
        while (elapsed < shootDuration)
        {
            RotateTowardsPlayer();  // 계속 회전

            // 탄 발사
            Vector3 dir = boss.player.position - boss.firePoint.position;
            dir.z = 0;
            dir.Normalize();

            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }

            // 회전 유지를 위한 대기 → interval 동안에도 프레임마다 회전
            float intervalTimer = 0f;
            while (intervalTimer < fireInterval)
            {
                RotateTowardsPlayer();
                intervalTimer += Time.deltaTime;
                yield return null;
            }

            elapsed += fireInterval;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = boss.player.position - boss.transform.position;
        dir.z = 0;
        dir.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, dir);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}
