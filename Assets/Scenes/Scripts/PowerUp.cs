using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Reset static count mỗi khi scene load lại để tránh tích lũy giữa các game session
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        ActiveCount = 0;
    }

    public static int ActiveCount { get; private set; }

    public PowerUpType type;
    public float fallSpeed = 2f;
    public AudioClip pickupSound;

    void Start()
    {
        ActiveCount++;
        Destroy(gameObject, 12f);
    }

    void OnDestroy()
    {
        ActiveCount = Mathf.Max(0, ActiveCount - 1);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (pickupSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        }

        PlayerController player = other.GetComponent<PlayerController>();
        GameManager gameManager = GameManager.Instance;

        if (type == PowerUpType.Heal)
        {
            if (gameManager != null)
            {
            }
        }
        else if (player != null)
        {
            player.ApplyPowerUp(type);
        }

        Destroy(gameObject);
    }
}
