using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Giao diện")]
    public GameObject pauseMenuPanel; // Bảng menu hiện ra khi ấn Pause
    public GameObject pauseButton;    // Nút Pause nhỏ góc màn hình

    private bool isPaused = false;

    void Start()
    {
        // Khi mới vào game, chắc chắn bảng Pause bị ẩn và thời gian trôi bình thường
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Gắn vào nút PAUSE
    public void PauseGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(false); // Ẩn nút Pause đi

        Time.timeScale = 0f; // Đóng băng game
        isPaused = true;
    }

    // Gắn vào nút PLAY (Tiếp tục)
    public void ResumeGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true); // Hiện lại nút Pause

        Time.timeScale = 1f; // Chạy lại game
        isPaused = false;
    }

    // Gắn vào nút Quit (Tùy chọn, để thoát ra Main Menu)
    public void QuitToMainMenu()
    {
        // QUAN TRỌNG: Phải trả lại thời gian về 1 trước khi chuyển Scene
        // Nếu không Main Menu của bạn cũng sẽ bị đóng băng!
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}