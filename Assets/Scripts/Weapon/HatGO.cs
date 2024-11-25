using UnityEngine;

[SerializeField]

/// <summary>
/// Hat GameObject class including Hat : Item composite class.
/// </summary>

public class HatGO : MonoBehaviour
{
    [SerializeField] public Hat hatData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(Hat hat)
    {
        this.hatData = hat;

        // Set up the sprite renderer based on the Hat's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null && hat != null)
        {
            spriteRenderer.sprite = hat.Sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
