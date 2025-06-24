using System.Collections;
using UnityEngine;

public class Boss1_Special : BossPattern
{
    public float initialDelay = 1f;
    public float fireInterval = 1f;
    public float acceleration = 0.01f;
    public float duration = 10f;
    public float bulletSpeed = 15f;
    public AudioClip soundeffect;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        yield return new WaitForSeconds(initialDelay);

        float timer = 0f;
        float currentInterval = fireInterval;

        while (timer < duration)
        {
            if (!boss || !boss.gameObject.activeSelf) yield break;
            if (!boss.player) yield break;

            // 회전 방향 설정
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector3 lookDir = new Vector3(randomDir.x, randomDir.y, 0f);
            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, lookDir);
            boss.transform.rotation = targetRot;

            // 탄환 발사
            Vector3 shootDir = boss.transform.up;
            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, shootDir);

            AudioManager.instance.PlaySFX(soundeffect);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = shootDir * bulletSpeed;

            yield return new WaitForSeconds(currentInterval);
            currentInterval = Mathf.Min(0.05f, currentInterval - acceleration);
            timer += currentInterval;
        }

        boss.EndSpecialPattern();
    }
}
