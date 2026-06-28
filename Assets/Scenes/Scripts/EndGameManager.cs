using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Header("Game Over Audio")]
    public AudioClip gameOverSound;

    private void Start()
    {
        if (gameOverSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);
        }

        int savedScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + savedScore.ToString("000");
        }

        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + highScore.ToString("000");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
