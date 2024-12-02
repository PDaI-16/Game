using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public GameObject Enemy;
    public float health;
    public float maxHealth;
    public float Damage;
    public int experienceReward = 100;
    private PlayerData playerData;

    void Start()
    {
        /*        playerData = GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerData;*/
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
            if (playerData != null)
            {
                playerData.AddXP(experienceReward);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy is continuously colliding with Player.");

            // Access the player's script
            playerData = collision.gameObject.GetComponent<PlayerController>().playerData;
            if (playerData != null)
            {
                // Continuously apply damage as long as the enemy is colliding with the player
                playerData.TakeDamage(Damage * Time.deltaTime); // Damage per second
            }
            else
            {
                Debug.LogWarning("PlayerStats script not found on Player.");
            }
        }
    }
}
