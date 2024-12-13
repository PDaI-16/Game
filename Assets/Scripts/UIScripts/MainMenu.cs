using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject selectPanel;
    public GameObject deathScreenPanel;
    public GameObject endScreenPanel;

    public static ItemCategory playerClass;

    [SerializeField] private PlayerController playerController;


    public void StartGame()
    {   
        Time.timeScale = 1;
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
        playerClass = ItemCategory.Melee;
        StartGame();
    }

    public void SelectRanged()
    {
        playerClass = ItemCategory.Ranged;
        StartGame();
    }

    public void SelectMagic()
    {
        playerClass = ItemCategory.Magic;
        StartGame();
    }

    public void BackToMenu()
    {
        deathScreenPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
