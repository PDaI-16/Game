using UnityEngine;
using UnityEngine.SceneManagement;
public class SkillTreeUI : MonoBehaviour
{

    public GameObject SkillTreePanel;
    public bool toggle;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Pressed K");
            SkillTreePanel.SetActive(!SkillTreePanel.activeSelf);
        }
    }
}
