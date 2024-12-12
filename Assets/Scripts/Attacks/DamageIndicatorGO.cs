using TMPro; // Make sure to include the TextMeshPro namespace
using UnityEngine;

public class DamageIndicatorGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI damageText;

/*
    damageText.text = "Hello, World!";*/

    void Start()
    {

    }
   
    // Update is called once per frame
    public void SetDamageText(float damage)
    {
        
        if (damageText != null)
        {
            damageText.text = damage.ToString("F2");
        }
    }

}

