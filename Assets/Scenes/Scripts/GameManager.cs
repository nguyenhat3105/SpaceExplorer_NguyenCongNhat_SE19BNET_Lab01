using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject asteroidPrefab;
    public GameObject starPrefab;
    public GameObject powerUpPrefab;
    public GameObject bossPrefab;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI highScoreText;

    [Header("Spawn Settings")]
    public float asteroidSpawnDelay = 2f;
    public float minAsteroidSpawnDelay = 0.9f;
    public float starMinDelay = 3f;
    public float starMaxDelay = 5f;
    public float powerUpMinDelay = 8f;
    public float powerUpMaxDelay = 14f;
    public int maxActiveAsteroids = 10;
    public int maxActiveStars = 5;
    public int maxActivePowerUps = 2;

    [Header("Level Settings")]
    [Tooltip("Số điểm cần để tăng 1 level (vd: 100 điểm = level 2, 200 = level 3...)")]
    public int scorePerLevel = 100;
    public int bossEveryLevel = 5;
    public float asteroidSpawnDelayDecreasePerLevel = 0.15f;

    [Header("Milestone Difficulty (Level 6, 11, 16, 21...)")]
    [Tooltip("Level đầu tiên kích hoạt milestone boost")]
    public int milestoneStartLevel = 6;
    [Tooltip("Cứ bao nhiêu level thì có 1 milestone")]
    public int milestoneInterval = 5;
    [Tooltip("Giảm thêm spawn delay tại mỗi milestone (tăng mật độ)")]
    public float milestoneSpawnDelayBonus = 0.25f;
    [Tooltip("Tốc độ rơi ban đầu của asteroid")]
    public float baseAsteroidFallSpeed = 3f;
    [Tooltip("Tăng tốc độ rơi mỗi milestone")]
    public float milestoneFallSpeedIncrease = 0.8f;
    [Tooltip("Tốc độ rơi tối đa")]
    public float maxAsteroidFallSpeed = 12f;

    [Header("--- BOSS UI ---")]
    public Slider bossHealthSlider;

    private int score = 0;
    private int level = 1;
    private bool bossActive;
    private Canvas gameplayCanvas;
    private float currentAsteroidFallSpeed;
    private GameObject currentBossInstance;

    public int Score => score;
    public int Level => level;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        // Ẩn boss health bar cho đến khi boss xuất hiện
        if (bossHealthSlider != null)
            bossHealthSlider.gameObject.SetActive(false);

        currentAsteroidFallSpeed = baseAsteroidFallSpeed;



        EnsureHudReferences();
        UpdateUI();

        StartCoroutine(SpawnAsteroidRoutine());
        StartCoroutine(SpawnStarRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        // Level tăng theo score trong AddScore()
    }

    private void TrySpawnBoss()
    {
        if (bossPrefab == null || bossActive || bossEveryLevel <= 0) return;

        if (level % bossEveryLevel == 0)
        {
            bossActive = true;
            currentBossInstance = Instantiate(bossPrefab, new Vector3(0f, 3.5f, 0f), Quaternion.identity);

            if (bossHealthSlider != null)
            {
                bossHealthSlider.gameObject.SetActive(true);
                bossHealthSlider.value = bossHealthSlider.maxValue;
            }
            Debug.LogWarning($"⚠️ [BOSS ALERT] Boss xuất hiện tại Level {level}!");
        }
    }

    public void UpdateBossHealthUI(int currentHealth)
    {
        if (bossHealthSlider != null)
            bossHealthSlider.value = currentHealth;
    }

    public void NotifyBossDestroyed()
    {
        bossActive = false;
        currentBossInstance = null;

        if (bossHealthSlider != null)
            bossHealthSlider.gameObject.SetActive(false);

        AddScore(200);
        Debug.Log("🎉 Boss đã bị tiêu diệt!");
    }

    private void ClearActiveBossIfAny()
    {
        if (bossActive)
        {
            bossActive = false;
            if (currentBossInstance != null)
            {
                Destroy(currentBossInstance);
                currentBossInstance = null;
            }

            if (bossHealthSlider != null)
                bossHealthSlider.gameObject.SetActive(false);

            Debug.Log("🛸 Người chơi đã vượt cấp! Tàu Boss cũ rút lui.");
        }
    }

    public void AddScore(int points)
    {
        score += points;

        // Kiểm tra level up dựa trên score
        // Level 1: 0–99 | Level 2: 100–199 | Level 3: 200–299 ...
        int newLevel = (scorePerLevel > 0) ? (score / scorePerLevel) + 1 : 1;
        if (newLevel > level)
        {
            int levelsGained = newLevel - level;
            for (int i = 0; i < levelsGained; i++)
            {
                level++;
                OnLevelUp();
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (scoreText != null) scoreText.text = "Score: " + score;
        if (levelText != null) levelText.text = "Level: " + level;
        if (highScoreText != null) highScoreText.text = "Best: " + highScore;
    }

    private void EnsureHudReferences()
    {
        gameplayCanvas = FindAnyObjectByType<Canvas>();
        if (gameplayCanvas == null) return;

        if (levelText == null) levelText = CreateHudText("LevelText", new Vector2(20f, -55f));
        if (highScoreText == null) highScoreText = CreateHudText("HighScoreText", new Vector2(20f, -90f));
    }

    private TextMeshProUGUI CreateHudText(string objectName, Vector2 anchoredPosition)
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(gameplayCanvas.transform, false);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.fontSize = 28f;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Left;
        text.raycastTarget = false;

        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(260f, 36f);

        return text;
    }

    /// <summary>Gọi từ Asteroid hoặc EnemyBullet để kết thúc game ngay lập tức.</summary>
    public void TriggerGameOver()
    {
        GameOver();
    }

    private void GameOver()
    {
        AudioSource cameraAudio = Camera.main != null ? Camera.main.GetComponent<AudioSource>() : null;
        if (cameraAudio != null) cameraAudio.Stop();

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore) PlayerPrefs.SetInt("HighScore", score);

        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndGame");
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (true)
        {
            if (bossActive)
                yield return new WaitUntil(() => !bossActive);

            if (asteroidPrefab != null && Asteroid.ActiveCount < maxActiveAsteroids)
            {
                float randomX = Random.Range(-8f, 8f);
                GameObject obj = Instantiate(asteroidPrefab, new Vector3(randomX, 6f, 0f), Quaternion.identity);

                Asteroid asteroid = obj.GetComponent<Asteroid>();
                if (asteroid != null)
                    asteroid.fallSpeed = currentAsteroidFallSpeed;
            }

            yield return new WaitForSeconds(asteroidSpawnDelay);
        }
    }

    IEnumerator SpawnStarRoutine()
    {
        while (true)
        {
            if (bossActive)
                yield return new WaitUntil(() => !bossActive);

            yield return new WaitForSeconds(Random.Range(starMinDelay, starMaxDelay));

            if (starPrefab != null && Star.ActiveCount < maxActiveStars)
            {
                float randomX = Random.Range(-8f, 8f);
                Instantiate(starPrefab, new Vector3(randomX, 6f, 0f), Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(powerUpMinDelay, powerUpMaxDelay));

            if (powerUpPrefab != null && PowerUp.ActiveCount < maxActivePowerUps)
            {
                float randomX = Random.Range(-8f, 8f);
                Instantiate(powerUpPrefab, new Vector3(randomX, 6f, 0f), Quaternion.identity);
            }
        }
    }

    /// <summary>Gọi mỗi lần level tăng. Áp dụng khó hơn và spawn boss nếu đến milestone.</summary>
    private void OnLevelUp()
    {
        ClearActiveBossIfAny();

        asteroidSpawnDelay = Mathf.Max(minAsteroidSpawnDelay,
            asteroidSpawnDelay - asteroidSpawnDelayDecreasePerLevel);

        // Milestone boost: Level 6, 11, 16, 21...
        bool isMilestone = level >= milestoneStartLevel
            && (level - milestoneStartLevel) % milestoneInterval == 0;

        if (isMilestone)
        {
            asteroidSpawnDelay = Mathf.Max(minAsteroidSpawnDelay,
                asteroidSpawnDelay - milestoneSpawnDelayBonus);

            currentAsteroidFallSpeed = Mathf.Min(maxAsteroidFallSpeed,
                currentAsteroidFallSpeed + milestoneFallSpeedIncrease);

            Debug.Log($"[Milestone] Level {level}: SpawnDelay={asteroidSpawnDelay:F2}s | FallSpeed={currentAsteroidFallSpeed:F1}");
        }

        Debug.Log($"Level Up! -> Level {level} (Score: {score})");
        UpdateUI();
        TrySpawnBoss();
    }
}