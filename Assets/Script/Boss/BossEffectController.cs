using System.Collections;
using UnityEngine;

public class BossEffectController : MonoBehaviour
{
    public Renderer bossRenderer;
    public Transform visualTransform;   // 흔들릴 비주얼 트랜스폼
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.4f;
    public float flashDuration = 0.2f;

    public Color flashColor = Color.white;
    public Color glowColor = Color.red;

    private Color originalColor;
    private Material bossMaterial;
    private Vector3 originalPosition;

    private void Awake()
    {
        //if (bossRenderer != null)
        //{
        //    bossMaterial = bossRenderer.material;
        //    originalColor = bossMaterial.color;
        //}

        //if (visualTransform != null)
        //{
        //    originalPosition = visualTransform.localPosition;
        //}
    }

    public void PlayHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FlashAndShake());
    }

    private IEnumerator FlashAndShake()
    {
        float elapsed = 0f;

        // 1. 플래시 색상 + Glow
        //if (bossMaterial != null)
        //{
        //    bossMaterial.color = flashColor;
        //    bossMaterial.EnableKeyword("_EMISSION");
        //    bossMaterial.SetColor("_EmissionColor", glowColor * 2f);
        //}

        // 2. 흔들림 시작
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

        // 원상복구
        if (bossMaterial != null)
        {
            bossMaterial.color = originalColor;
            bossMaterial.SetColor("_EmissionColor", Color.black);
            bossMaterial.DisableKeyword("_EMISSION");
        }

        if (visualTransform != null)
        {
            visualTransform.localPosition = originalPosition;
        }
    }
}
