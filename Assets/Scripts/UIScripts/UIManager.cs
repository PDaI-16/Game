using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject hintPanel;

    private List<GameObject> panels = new List<GameObject>();

    // Define the states of the FSM
    private enum PanelState
    {
        None,
        SkillTree,
        Inventory
    }

    void Start()
    {
        // Add panels to the list, except the hintpanel
        panels.Add(skillTreePanel);
        panels.Add(inventoryPanel);
    }

    void Update()
    {
        // Check for input and handle state transitions
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Pressed K");
            ChangeState(PanelState.SkillTree);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Pressed I");
            ChangeState(PanelState.Inventory);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(PanelState.None);
        }
    }

    public void ActivateHintPanel()
    {
        hintPanel.SetActive(true);
    }

    public void HideHintPanel()
    {
        hintPanel.SetActive(false);
    }

    // Change the FSM state
    private void ChangeState(PanelState newState)
    {
        // Activate the panel based on the new state
        switch (newState)
        {
            case PanelState.SkillTree:
                DeactivateAllExludingThis(skillTreePanel);
                skillTreePanel.SetActive(!skillTreePanel.activeSelf);
                break;

            case PanelState.Inventory:
                DeactivateAllExludingThis(inventoryPanel);
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                break;

            case PanelState.None:
                DeactivateAllPanels();
                break;
        }

    }

    private void DeactivateAllExludingThis(GameObject usedPanel)
    {
        foreach (var panel in panels)
        {
            if (panel != usedPanel)
            {
                panel.SetActive(false);
            }
           
        }
    }

    private void DeactivateAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
