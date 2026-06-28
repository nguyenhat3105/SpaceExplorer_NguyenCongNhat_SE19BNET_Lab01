using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public int health = 30;

    [Header("Movement")]
    public float horizontalRange = 5f;
    public float horizontalSpeed = 2f;

    [Header("Attack")]
    public GameObject enemyBulletPrefab;
    public float fireRate = 1f;

    [Header("Audio")] // Thêm phần khai báo âm thanh
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Validation")]
    [Tooltip("Nếu true: boss tự hủy khi game chưa đạt milestone level hợp lệ để spawn boss")]
    public bool destroyIfLevelTooLow = true;

    // --- Private ---
    private float nextFireTime;
    private bool isInvincible;        // Chống nhận nhiều hit cùng frame
    private float invincibleUntil;
    private const float HIT_COOLDOWN = 0.05f;  // 50ms giữa mỗi lần nhận damage

    void Start()
    {
        // Lấy component AudioSource tự động
        audioSource = GetComponent<AudioSource>();

        // Chỉ hủy nếu chưa đạt level milestone đầu tiên hợp lệ để spawn boss
        if (destroyIfLevelTooLow)
        {
            GameManager gm = GameManager.Instance;
            bool levelValid = gm == null
                || gm.bossEveryLevel <= 0
                || (gm.Level >= gm.bossEveryLevel && gm.Level % gm.bossEveryLevel == 0);

            if (!levelValid)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void Update()
    {
        // Dao động ngang sin wave
        transform.position = new Vector3(
            Mathf.Sin(Time.time * horizontalSpeed) * horizontalRange,
            3.5f,
            0f
        );

        // Bắn đạn
        if (enemyBulletPrefab != null && Time.time >= nextFireTime)
        {
            Instantiate(enemyBulletPrefab, transform.position + Vector3.down, Quaternion.identity);
            nextFireTime = Time.time + fireRate;

            // Phát âm thanh bắn
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Laser")) return;

        // Invincibility frame: chống nhiều laser hit cùng 1 frame vật lý
        if (Time.time < invincibleUntil) return;
        invincibleUntil = Time.time + HIT_COOLDOWN;

        Destroy(other.gameObject);
        health--;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateBossHealthUI(health);
        }

        if (health <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NotifyBossDestroyed();
            }
            Destroy(gameObject);
        }
    }
}