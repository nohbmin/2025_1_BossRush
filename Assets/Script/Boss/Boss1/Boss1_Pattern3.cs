using System.Collections;
using UnityEngine;

public class Boss1_Pattern3 : BossPattern
{
    public float FirstSpreadAngle = 60f;
    public int FirstBulletCount = 10;
    public int upgradeHp = 2;
    public float bulletSpeed; 
    public float waitingtime;
    public float ChaseTime = 1.5f;
    public AudioClip soundEffect;

    public override IEnumerator ExecutePattern(int currentHP, int maxHP)
    {
        float spreadAngle = FirstSpreadAngle;
        int bulletCount = FirstBulletCount;

        print(currentHP + " 와 " + upgradeHp);
        if (currentHP <= upgradeHp)
        {
            print("3스 강화");
            spreadAngle *= 2;
            bulletCount *= 3;
        }

        float chaseing = ChaseTime;

        while (chaseing > 0)
        {
            Vector3 dir = (boss.player.position - boss.transform.position);
            dir.z = 0; // XY ��� ����
            dir.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, dir); // XY ���� ȸ��
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRot, Time.deltaTime * 5f);
            chaseing -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(waitingtime);

        // źȯ �߻�
        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float speed = Random.Range(1f, 3f);

            Vector3 baseDir = boss.transform.up; // XY ��� ���� forward �� up
            Vector3 rotatedDir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

            GameObject bullet = boss.bulletPool.GetBullet();
            AudioManager.instance.PlaySFX(soundEffect);
            boss.animator.Play("Gun_Shoot");
            bullet.transform.position = boss.firePoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedDir); // XY ���� ȸ��

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = rotatedDir * speed * bulletSpeed;
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
