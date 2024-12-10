using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.0f;
    public float projectileSpeed = 10.0f;
    public float destroyTime = 3.0f;

    private float fireCooldown = 0.0f;
    private RangedMovement rangedMovement;
    private GameObject player;

    private void Start()
    {
        // Get the RangedMovement component
        rangedMovement = GetComponent<RangedMovement>();
        if (rangedMovement == null)
        {
            Debug.LogError("RangedMovement component not found on the GameObject.");
        }

        // Check if firePoint is assigned
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned.");
        }

        // Get reference to the player
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        // Decrease the cooldown timer
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }

        // Check if we can attack
        if (rangedMovement != null && rangedMovement.isInGracePeriod && fireCooldown <= 0f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null || player == null)
        {
            Debug.LogWarning("ProjectilePrefab, FirePoint, or Player is missing.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the projectilePrefab.");
            return;
        }

        rb.gravityScale = 0;  

        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;

        float angleOffset = -45f; // Adjust this value to match your prefab's alignment
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + angleOffset;

        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.linearVelocity = directionToPlayer * projectileSpeed;

        Collider2D col = projectile.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D component is missing from the projectilePrefab.");
            return;
        }

        // Destroy the projectile after a certain time (3 seconds)
        Destroy(projectile, destroyTime);
        fireCooldown = fireRate;

        Debug.Log("Enemy fired a projectile!");
    }
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

    // When the projectile collides with something
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name); // Debug log to see what it's colliding with
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            // Deal damage to the player
            PlayerData playerData = other.GetComponent<PlayerData>();
            if (playerData != null)
            {
                playerData.TakeDamage(10f);  // Adjust the damage value as needed
                Debug.Log("Projectile dealt damage to the player!");
            }
            Destroy(gameObject); // Destroy the projectile
        }
    }

}
