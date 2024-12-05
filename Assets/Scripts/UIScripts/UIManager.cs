using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private GameObject inventoryPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Pressed K");
            skillTreePanel.SetActive(!skillTreePanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Pressed I");
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
