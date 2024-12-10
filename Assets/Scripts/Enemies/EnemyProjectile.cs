using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;  // The amount of damage the projectile does

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile collided with the player
        if (other.CompareTag("Player"))
        {
            // Access the player GameObject via the Singleton reference
            GameObject player = PlayerReference.Instance;

            if (player != null)
            {
                // Assuming PlayerController is attached to the player, you can now access it
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerController != null)
                {
                    // Access player data and apply damage
                    playerController.playerData.TakeDamage(damage);  
                    Debug.Log("Projectile hit player and dealt damage!");
                }
                else
                {
                    Debug.LogError("PlayerController not found on the player.");
                }
            }
            else
            {
                Debug.LogError("Player instance is null. Ensure PlayerReference is set up correctly.");
            }

            // Destroy the projectile upon impact
            Destroy(gameObject);
        }
    }

    // Optionally, add behavior for when the projectile goes out of bounds, or times out:
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
