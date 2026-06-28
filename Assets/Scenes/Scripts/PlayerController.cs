using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Combat")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public AudioClip shootSound;

    [Header("Movement Limit")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Skins")]
    public Sprite[] shipSkins;

    [Header("Power Up Visuals")]
    public GameObject shieldVisual;   // Kéo child 'Shield' vào đây trong Inspector
    [Tooltip("Player bắt đầu game với 1 lớp shield miễn phí")]
    public bool startWithShield = true;

    private Rigidbody2D rb;
    private float nextFireTime;
    private float originalSpeed;
    private bool hasShield;
    private bool doubleShot;
    private SpriteRenderer spriteRenderer;
    private Color originalColor = Color.white;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        originalSpeed = moveSpeed;

        int selectedSkin = PlayerPrefs.GetInt("SelectedSkin", 0);
        if (spriteRenderer != null && shipSkins != null && selectedSkin >= 0 && selectedSkin < shipSkins.Length)
            spriteRenderer.sprite = shipSkins[selectedSkin];

        // Bắt đầu game với shield nếu được bật
        hasShield = startWithShield;
        UpdateShieldVisual();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(moveX, moveY).normalized * moveSpeed;

        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);
        transform.position = currentPos;

        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ApplyPowerUp(PowerUpType type)
    {
        if (type == PowerUpType.Shield)
        {
            hasShield = true;
            UpdateShieldVisual();
        }
        else if (type == PowerUpType.SpeedBoost)
        {
            StopCoroutine(nameof(SpeedBoostRoutine));
            StartCoroutine(nameof(SpeedBoostRoutine));
        }
        else if (type == PowerUpType.DoubleShot)
        {
            StopCoroutine(nameof(DoubleShotRoutine));
            StartCoroutine(nameof(DoubleShotRoutine));
        }
    }

    /// <summary>Dùng shield để chặn 1 đòn tấn công. Trả về true nếu shield đang active.</summary>
    public bool TryUseShield()
    {
        if (!hasShield) return false;

        hasShield = false;
        UpdateShieldVisual();
        return true;
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        if (shootSound != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);

        if (doubleShot)
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 0.8f, 0f), Quaternion.Euler(0, 0, 90));
            Instantiate(bulletPrefab, transform.position + new Vector3(0.3f, 0.8f, 0f), Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(0f, 0.8f, 0f), Quaternion.Euler(0, 0, 90));
        }
    }

    IEnumerator SpeedBoostRoutine()
    {
        moveSpeed = originalSpeed * 1.8f;
        yield return new WaitForSeconds(6f);
        moveSpeed = originalSpeed;
    }

    IEnumerator DoubleShotRoutine()
    {
        doubleShot = true;
        yield return new WaitForSeconds(7f);
        doubleShot = false;
    }

    private void UpdateShieldVisual()
    {
        // Ẩn/hiện shield object con
        if (shieldVisual != null)
            shieldVisual.SetActive(hasShield);

        // Đổi màu tàu: xanh lam khi có shield, màu gốc khi không có
        if (spriteRenderer != null)
            spriteRenderer.color = hasShield ? new Color(0.35f, 0.85f, 1f, 1f) : originalColor;
    }
}
