using UnityEngine;

public class Boss1_bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            // ��Ȱ��ȭ �� Ǯ�� ��ȯ
            var pool = GetComponentInParent<BulletPool>();
            if (pool != null)
            {
                pool.ReturnBullet(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�ÿ��� Ǯ�� ��ȯ ó��
        var pool = GetComponentInParent<BulletPool>();
        if (pool != null)
        {
            pool.ReturnBullet(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
