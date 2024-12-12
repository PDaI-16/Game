using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public GameObject MeleeEnemyPrefab;    // Prefab for melee minions
    public GameObject RangedEnemyPrefab;   // Prefab for ranged minions
    public int numberOfMinionsToSummon = 3; // Number of minions to summon during special attack
    public float summonRadius = 5f;        // Radius around the boss where minions are summoned
    public float speed = 5f;               // Movement speed of the boss
    public float aggroStartDistance = 15f; // Distance at which the boss becomes aggroed
    public float aggroEndDistance = 20f;   // Distance at which the boss loses aggro
    public float meleeAttackDistance = 2f; // Ideal distance for melee attacks
    public float rangedSafeDistance = 10f; // Distance to keep for ranged attacks
    public float attackCooldown = 2f;     // Time between each attack
    public float attackGraceTime = 1f;    // Time the boss waits after exiting attack range

    private GameObject player;             // Player reference (will be accessed via Singleton)
    private float distanceToPlayer;        // Current distance to the player
    private bool isAggroed;                // Tracks if the boss is aggroed
    private bool isInGracePeriod;          // Tracks if the boss is in the grace period after an attack
    private float graceTimer;              // Tracks time left in the grace period
    private bool canAttack = true;         // Flag to track if the boss can attack again

    private EnemyStats enemyStats;         // Reference to the EnemyStats component
    private RangedAttack rangedAttack; // Add this to your boss script

    private void Start()
    {
        // Access the player reference from the PlayerSingleton (singleton pattern)
        player = PlayerReference.Instance;

        if (player == null)
        {
            Debug.LogError("Player reference not found. Ensure the PlayerSingleton script is attached to the player.");
        }

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
            if (isAggroed && !isInGracePeriod)
            {
                ChooseAction(distanceToPlayer);
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

    // Choose an action based on the distance to the player
    private void ChooseAction(float distanceToPlayer)
    {
        if (canAttack)
        {
            if (distanceToPlayer <= meleeAttackDistance)
            {
                PerformMeleeAttack();
            }
            else if (distanceToPlayer <= rangedSafeDistance)
            {
                PerformRangedAttack();
            }
            else
            {
                PerformSpecialMove(); // Boss executes a special move at longer ranges
            }
        }
        else
        {
            MoveTowardsPlayer(distanceToPlayer); // Move closer if no immediate action can be performed
        }
    }

    // Move towards the player
    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Melee attack logic
    private void PerformMeleeAttack()
    {
        Debug.Log("Melee Attack!");
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;

        // Add melee attack logic (e.g., damage to player, trigger animation, etc.)
        Invoke("ResetAttackCooldown", attackCooldown);
    }

    // Ranged attack logic
    private void PerformRangedAttack()
    {
        Debug.Log("Ranged Attack!");
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;

        // Call the RangedAttack component's Attack method
        if (rangedAttack != null)
        {
            rangedAttack.Attack(); // Calls the Attack() method from RangedAttack script
        }
        else
        {
            Debug.LogError("RangedAttack component is missing.");
        }

        // Reset the attack cooldown
        Invoke("ResetAttackCooldown", attackCooldown);
    }

    // Special move logic
    private void PerformSpecialMove()
    {
        Debug.Log("Special Move: Summon Minions!");
        SummonMinions(); // Call the summon minions special attack

        // Prevent immediate re-use of the special move
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;
        Invoke("ResetAttackCooldown", attackCooldown);
    }

    // Summon minions logic
    private void SummonMinions()
    {
        for (int i = 0; i < numberOfMinionsToSummon; i++)
        {
            // Randomly choose between melee and ranged minion
            GameObject minionPrefab = Random.Range(0, 2) == 0 ? MeleeEnemyPrefab : RangedEnemyPrefab;

            // Calculate a random spawn position within a radius around the boss
            Vector2 randomOffset = Random.insideUnitCircle * summonRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            // Instantiate the chosen minion prefab at the spawn position
            Instantiate(minionPrefab, spawnPosition, Quaternion.identity);
        }
        Debug.Log($"Summoned {numberOfMinionsToSummon} minions!");
    }

    // Reset attack cooldown, allowing the boss to attack again
    private void ResetAttackCooldown()
    {
        canAttack = true;
    }
    public class ProjectileMoveToPlayer : MonoBehaviour
    {
        public Transform target; // The target (player)
        public float projectileSpeed = 10f; // Speed of the projectile

        private void Update()
        {
            if (target != null)
            {
                // Move the projectile towards the player
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * projectileSpeed * Time.deltaTime;
            }
        }
    }
}