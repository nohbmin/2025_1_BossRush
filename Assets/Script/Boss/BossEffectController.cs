using System.Collections;
using UnityEngine;

public class BossEffectController : MonoBehaviour
{
    public Renderer bossRenderer;
    public Transform visualTransform;   // Èçµé¸± ºñÁÖ¾ó Æ®·£½ºÆû
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.4f;
    public float flashDuration = 0.2f;

    private Vector3 originalPosition;

    private void Awake()
    {
        if (visualTransform != null)
        {
            originalPosition = visualTransform.localPosition;
        }
    }

    public void PlayHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FlashAndShake());
    }

    private IEnumerator FlashAndShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            if (visualTransform != null)
            {
                Vector3 offset = Random.insideUnitSphere * shakeIntensity;
                offset.z = 0;
                visualTransform.localPosition = originalPosition + offset;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (visualTransform != null)
        {
            visualTransform.localPosition = originalPosition;
        }
    }
}
