using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Tốc độ cuộn nền")]
    public float speed = 2f;

    private Material bgMaterial;

    void Start()
    {
        // Lấy Material từ Mesh Renderer của Quad thay vì SpriteRenderer
        bgMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Tăng dần tọa độ Y của Texture theo thời gian để tạo hiệu ứng cuộn vô tận
        Vector2 offset = bgMaterial.mainTextureOffset;
        offset.y += speed * Time.deltaTime;
        bgMaterial.mainTextureOffset = offset;
    }
}