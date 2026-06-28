using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    public float blinkSpeed = 6f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        // Hiệu ứng nhấp nháy nhẹ khi shield active
        float alpha = 0.55f + Mathf.Sin(Time.time * blinkSpeed) * 0.25f;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    // Reset alpha khi bị tắt để lần sau bật lại đẹp
    void OnDisable()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }
}
