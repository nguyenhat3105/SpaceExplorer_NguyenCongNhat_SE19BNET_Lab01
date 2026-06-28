using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Chạm mũi tên boss → kết thúc game ngay lập tức
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.TriggerGameOver();
        }

        Destroy(gameObject);
    }
}
