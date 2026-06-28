using UnityEngine;

public class Star : MonoBehaviour
{
    // Reset static count mỗi khi scene load lại để tránh tích lũy giữa các game session
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        ActiveCount = 0;
    }

    public static int ActiveCount { get; private set; }

    [Header("Speed")]
    public float fallSpeed = 3f;

    [Header("Score")]
    public int scoreValue = 10;

    [Header("Audio")]
    public AudioClip collectSound;

    void Start()
    {
        ActiveCount++;
    }

    void OnDestroy()
    {
        ActiveCount = Mathf.Max(0, ActiveCount - 1);
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (collectSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
        }

        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }
}
