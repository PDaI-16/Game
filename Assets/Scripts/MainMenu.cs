using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject selectPanel;
    public PlayerStats playerStats;
    public static string selectedClass;


    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
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
    public void OpenSelect()
    {
        selectPanel.SetActive(true);
    }
    public void CloseSelect()
    {
        selectPanel.SetActive(false);
    }
    public void SelectMelee()
    {
        selectedClass = "Melee";
        StartGame();
    }

    public void SelectRanged()
    {
        selectedClass = "Ranged";
        StartGame();
    }

    public void SelectMagic()
    {
        selectedClass = "Magic";
        StartGame();
    }
}
