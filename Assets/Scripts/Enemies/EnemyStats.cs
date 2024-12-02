using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public GameObject Enemy;
    public float health;
    public float maxHealth;
    public float Damage;
    public int experienceReward = 100; 

    [SerializeField] private PlayerStats playerStats;

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

    private void Update()
    {
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (health <= 0)
        {
            if (playerStats != null)
            {
                playerStats.GainXP(experienceReward);
            }
            else
            {
                Debug.LogError("playerStats is null, can't award XP.");
            }
            Destroy(gameObject);
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
