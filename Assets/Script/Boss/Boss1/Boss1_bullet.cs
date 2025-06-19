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
            // 비활성화 후 풀에 반환
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
        // 충돌시에도 풀에 반환 처리
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
