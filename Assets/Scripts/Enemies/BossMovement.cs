using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public GameObject MeleeEnemyPrefab;
    public GameObject RangedEnemyPrefab;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int numberOfMinionsToSummon = 3;
    public float summonRadius = 5f;
    public float speed = 5f;
    public float aggroStartDistance = 15f;
    public float aggroEndDistance = 20f;
    public float meleeAttackDistance = 2f;
    public float rangedSafeDistance = 10f;
    public float attackCooldown = 2f;
    public float attackGraceTime = 1f;
    public float projectileSpeed = 10f;
    public float projectileDestroyTime = 5f;

    private GameObject player;
    private float distanceToPlayer;
    private bool isAggroed;
    private bool isInGracePeriod;
    private float graceTimer;
    private bool canAttack = true;

    private EnemyStats enemyStats;


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

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned for ranged attacks.");
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
                PerformSpecialMove(); 
            }
        }
        else
        {
            MoveTowardsPlayer(distanceToPlayer);
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
        if (projectilePrefab == null || firePoint == null || player == null)
        {
            Debug.LogWarning("ProjectilePrefab, FirePoint, or Player is missing.");
            return;
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, new Vector3(firePoint.position.x, firePoint.position.y, 0), Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the projectilePrefab.");
            return;
        }

        rb.gravityScale = 0;

        // Calculate direction to the player
        Vector2 displacement = player.transform.position - firePoint.position;
        if (displacement.magnitude < 0.1f) // Prevent very small displacement from causing issues
        {
            displacement = Vector2.right; // Default direction if too close (e.g., to the right)
            Debug.LogWarning("FirePoint and Player are too close. Defaulting direction to the right.");
        }

        Vector2 directionToPlayer = displacement.normalized;

        // Calculate the angle for rotation
        float angleOffset = -45f; 
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + angleOffset;

        // Apply rotation and velocity to the projectile
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.linearVelocity = directionToPlayer * projectileSpeed;

        Destroy(projectile, projectileDestroyTime);

        Debug.Log("Boss fired a projectile!");

        // Attack cooldown handling
        canAttack = false;
        isInGracePeriod = true;
        graceTimer = attackGraceTime;
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
}
