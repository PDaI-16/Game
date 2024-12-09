using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public GameObject player;             // Reference to the player GameObject
    public float speed = 5f;              // Movement speed of the boss
    public float aggroStartDistance = 15f;  // Distance at which the boss becomes aggroed
    public float aggroEndDistance = 20f;    // Distance at which the boss loses aggro
    public float meleeAttackDistance = 2f;  // Ideal distance for melee attacks
    public float rangedSafeDistance = 10f;  // Distance to keep for ranged attacks
    public float attackCooldown = 2f;      // Time between each attack
    public float attackGraceTime = 1f;     // Time the boss waits after exiting attack range

    private float distanceToPlayer;        // Current distance to the player
    private bool isAggroed;                // Tracks if the boss is aggroed
    private bool isInGracePeriod;          // Tracks if the boss is in the grace period after an attack
    private float graceTimer;              // Tracks time left in the grace period
    private bool canAttack = true;         // Flag to track if the boss can attack again

    private EnemyStats enemyStats;         // Reference to the EnemyStats component

    private void Start()
    {
        // Get the EnemyStats component attached to the boss
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component is missing on this boss!");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the distance to the player
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // Check if the boss should become aggroed or lose aggro
            if (!isAggroed && distanceToPlayer < aggroStartDistance)
            {
                isAggroed = true;
            }
            else if (isAggroed && distanceToPlayer > aggroEndDistance)
            {
                isAggroed = false;
            }

            // Handle movement and behavior logic based on aggro status
            if (isAggroed)
            {
                if (enemyStats.isMelee)
                {
                    // If the boss is melee, move towards the player
                    MoveTowardsPlayer(distanceToPlayer);
                }
                else
                {
                    // If the boss is ranged, maintain a safe distance
                    KeepSafeDistanceFromPlayer(distanceToPlayer);
                }
            }

            // Handle grace period countdown after an attack
            if (isInGracePeriod)
            {
                graceTimer -= Time.deltaTime;

                // End the grace period if the timer runs out
                if (graceTimer <= 0f)
                {
                    isInGracePeriod = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("Player does not exist.");
        }
    }

    // Move towards the player if it's a melee boss
    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > meleeAttackDistance && !isInGracePeriod)
        {
            // Move towards the player until reaching the melee attack distance
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (distanceToPlayer <= meleeAttackDistance && canAttack)
        {
            // If the player is within melee range, perform a melee attack
            PerformMeleeAttack();
        }
    }

    // Keep a safe distance from the player if it's a ranged boss
    private void KeepSafeDistanceFromPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer < rangedSafeDistance && !isInGracePeriod)
        {
            // Move away from the player to maintain the safe distance
            Vector2 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.deltaTime);
        }
        else if (distanceToPlayer >= rangedSafeDistance && canAttack)
        {
            // If the boss is at the ideal ranged attack distance, perform a ranged attack
            PerformRangedAttack();
        }
    }

    // Melee attack logic (you can expand this with animations or damage logic)
    private void PerformMeleeAttack()
    {
        Debug.Log("Melee Attack!");
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;  // Start grace period after attack

        // Add attack logic (e.g., damage to player, trigger animation, etc.)
        Invoke("ResetAttackCooldown", attackCooldown);  // Reset cooldown after some time
    }

    // Ranged attack logic (you can expand this with shooting mechanics, animations, etc.)
    private void PerformRangedAttack()
    {
        Debug.Log("Ranged Attack!");
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;  // Start grace period after attack

        // Add ranged attack logic (e.g., shooting projectiles, casting spells, etc.)
        Invoke("ResetAttackCooldown", attackCooldown);  // Reset cooldown after some time
    }

    // Reset attack cooldown, allowing the boss to attack again
    private void ResetAttackCooldown()
    {
        canAttack = true;
    }
}
