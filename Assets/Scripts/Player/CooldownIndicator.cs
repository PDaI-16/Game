using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private PlayerController playerController;


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

        if(cooldownPercent == previousPercent)
        {
            return;
        }

        previousPercent = cooldownPercent;
        cooldownSlider.value = cooldownPercent;

    }
}
