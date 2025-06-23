using System.Collections;
using UnityEngine;

public class BossEffectController : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.3f;



    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void PlayHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            randomOffset.z = 0; // XY 평면에서만 흔들기
            transform.localPosition = originalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}