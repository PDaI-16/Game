using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;  // The amount of damage the projectile does

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile collided with the player
        if (other.CompareTag("Player"))
        {
            // Access the player's data to deal damage
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.playerData.TakeDamage(damage);  // Deal the damage
                Debug.Log("Projectile hit player and dealt damage!");
            }

            // Destroy the projectile upon impact
            Destroy(gameObject);
        }
    }

    // Optionally, you can add behavior for when the projectile goes out of bounds, or times out:
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
