using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public GameObject Enemy; // Reference to the enemy GameObject
    public float health;
    public float maxHealth;
    public float Damage;
    [SerializeField] private PlayerStats playerStats; // Reference to PlayerStats (assign in Inspector or find dynamically)

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {health}");
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (health <= 0)
        {
            Debug.Log("Enemy died!");
            Destroy(Enemy);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy is continuously colliding with Player.");

            // Continuously apply damage as long as the enemy is colliding with the player
            playerStats.TakeDamage(Damage * Time.deltaTime);  // Damage per second, using deltaTime to scale it properly
        }
    }
}
