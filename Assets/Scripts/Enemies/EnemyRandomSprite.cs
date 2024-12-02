using UnityEngine;

public class EnemyRandomSprite : MonoBehaviour
{
    public Sprite[] enemySprites;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite();
    }

    void SetRandomSprite()
    {
        if (enemySprites.Length > 0)
        {
            // Select a random sprite
            Sprite randomSprite = enemySprites[Random.Range(0, enemySprites.Length)];

            // Apply the sprite to the SpriteRenderer
            spriteRenderer.sprite = randomSprite;
        }
        else
        {
            Debug.LogWarning("No sprites assigned to the enemySprites array.");
        }
    }
}
