using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Level 01");
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
