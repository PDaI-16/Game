using TMPro; // Make sure to include the TextMeshPro namespace
using UnityEngine;

public class DamageIndicatorGO : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float lifetime = 1.5f;       // Total lifetime before destruction
    [SerializeField] private float fadeDuration = 0.5f;   // Duration of the fade-out

    private float fadeStartTime;

    void Start()
    {
        fadeStartTime = lifetime - fadeDuration; // Start fading out before the object is destroyed
        Invoke(nameof(StartFade), fadeStartTime); // Schedule the fade-out
        Destroy(gameObject, lifetime);           // Schedule the destruction
    }

    public void SetDamageText(float damage)
    {
        if (damageText != null)
        {
            damageText.text = "-"+damage.ToString("F2");
        }
    }

    private void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float startAlpha = damageText.color.a; // Current alpha value
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the text is completely transparent
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 0f);
    }
}

