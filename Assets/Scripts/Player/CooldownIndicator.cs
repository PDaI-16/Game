using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject fill;


    private float previousPercent = 0.0f;

    void Start()
    {
        
        UpdateCooldownSlider();
    }

    private void Update()
    {
        UpdateCooldownSlider();
    }

    public void UpdateCooldownSlider()
    {
        // Calculate the health percentage
        float cooldownPercent = playerController.GetCoolDownTimer() / playerController.GetCoolDownTime();

        if (cooldownPercent == 1)
        {
            cooldownPercent = 0;
        }

        if (cooldownPercent == previousPercent)
        {
            return;
        }

        Debug.LogWarning("Cooldown percent: "+cooldownPercent);

        if (cooldownPercent <= 0.0f && fill.activeSelf == true) {
            fill.SetActive(false);
        }
        else if (cooldownPercent > 0.0f && fill.activeSelf == false)
        {
            fill.SetActive(true);
        }

        previousPercent = cooldownPercent;
        cooldownSlider.value = cooldownPercent;

    }
}
