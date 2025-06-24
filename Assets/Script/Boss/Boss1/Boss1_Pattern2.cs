using System.Collections;
using UnityEngine;

public class Boss1_Pattern2 : BossPattern
{
    public float waitBeforeFire = 1.5f;
    public float fireInterval = 0.5f;
    public float bulletSpeed = 7f;
    public float rotationSpeed = 5f;
    public float rotateDuration = 0.3f;

    public GameObject warningSign; // 느낌표 프리팹
    public float warningDistance = 3f; // 보스 기준 거리
    public float warningDuration = 0.5f;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        // 시작 각도 (대각선 중 하나)
        float[] diagonals = { 45f, 135f, 225f, 315f };
        float startAngle = diagonals[Random.Range(0, diagonals.Length)];

        // 회전 방향
        int direction = Random.value > 0.5f ? 1 : -1;


        // ⚠️ 경고 오브젝트 생성
        Quaternion firstRot = Quaternion.Euler(0, 0, startAngle);
        Vector3 warnDir = firstRot * Vector3.up;
        Vector3 warnPos = boss.transform.position + warnDir.normalized * warningDistance;

        GameObject warn = GameObject.Instantiate(warningSign, warnPos, Quaternion.identity);
        print(warn.name);
        yield return new WaitForSeconds(waitBeforeFire);
        print(waitBeforeFire);
        GameObject.Destroy(warn);
        print("destoryed");

        for (int i = 0; i < 4; i++)
        {
            float angle = startAngle + direction * i * 90f;
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);

            // 자연스러운 회전
            float t = 0f;
            Quaternion startRot = boss.transform.rotation;
            while (t < 1f)
            {
                t += Time.deltaTime * rotationSpeed;
                boss.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
                yield return null;
            }

            // 탄환 발사
            Vector3 dir = targetRot * Vector3.up;

            GameObject bullet = boss.bulletPool.GetBullet();
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = targetRot;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
}
