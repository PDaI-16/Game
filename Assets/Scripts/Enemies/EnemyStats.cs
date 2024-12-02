using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float Damage;
    public int experienceReward = 100; 

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyHealthBar enemyHealthBar;
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {health}");
        CheckDeath();
        enemyHealthBar.UpdateHealthBar();
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats.TakeDamage(Damage * Time.deltaTime);
        }
    }
}
