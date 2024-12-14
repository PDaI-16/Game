using UnityEngine;
using UnityEngine.SceneManagement;
public class SkillTreeUI : MonoBehaviour
{

    public GameObject SkillTreePanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Pressed K");
            SkillTreePanel.SetActive(!SkillTreePanel.activeSelf);
        }
    }
}
