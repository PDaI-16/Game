using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10; // Amount of damage dealt per attack
    public EnemyStats enemyStats; // Reference to the enemy's stats script (to apply damage)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button (M1)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (enemyStats != null)
        {
            // Deal damage to the enemy
            enemyStats.TakeDamage(attackDamage);
            Debug.Log($"Dealt {attackDamage} damage to the enemy!");
        }
        else
        {
            Debug.LogWarning("No enemy assigned to attack!");
        }
    }
}
