using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinSelector : MonoBehaviour
{
    public string gameplaySceneName = "Gameplay";

    public void SelectSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("SelectedSkin", skinIndex);
        PlayerPrefs.Save();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}
