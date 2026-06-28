using UnityEngine;

public class Asteroid : MonoBehaviour
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
    public float rotateSpeed = 50f;

    [Header("Damage")]
    public int damage = 1;

    [Header("Score")]
    public int scoreWhenDestroyed = 5;

    [Header("Audio")]
    public AudioClip crashSound;

    void Start()
    {
        ActiveCount++;
        Destroy(gameObject, 10f);
    }

    void OnDestroy()
    {
        ActiveCount = Mathf.Max(0, ActiveCount - 1);
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayCrashFeedback();

            // Chạm thiên thạch → kết thúc game ngay lập tức
            GameManager gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.TriggerGameOver();
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            PlayCrashFeedback();

            GameManager gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.AddScore(scoreWhenDestroyed);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void PlayCrashFeedback()
    {
        if (crashSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position);
        }

        if (Camera.main == null)
        {
            return;
        }

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.Shake(0.2f, 0.5f);
        }
    }
}
