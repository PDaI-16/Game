using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab to shoot
    public Transform firePoint;          // The point from which the projectile is fired
    public float fireRate = 1.0f;        // Time between attacks in seconds
    public float projectileSpeed = 10.0f; // Speed of the projectile
    public float destroyTime = 3.0f;     // Time before the projectile destroys itself

    private float fireCooldown = 0.0f;   // Tracks time left until the next attack
    private RangedMovement rangedMovement; // Reference to the RangedMovement component
    private GameObject player;           // Reference to the player

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

        // Debug log to confirm projectile instantiation
        Debug.Log("Instantiating projectile at position: " + firePoint.position);

        // Instantiate the projectile at the firePoint (enemy's position)
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component from the projectilePrefab
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the projectilePrefab.");
            return;
        }

        rb.gravityScale = 0;  // Prevent gravity from affecting the projectile

        // Get the direction towards the player
        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;

        // Set the velocity towards the player
        rb.linearVelocity = directionToPlayer * projectileSpeed;

        // Ensure the projectile has a collider
        Collider2D col = projectile.GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D component is missing from the projectilePrefab.");
            return;
        }

        // Destroy the projectile after a certain time (3 seconds)
        Destroy(projectile, destroyTime);

        // Reset the fire cooldown
        fireCooldown = fireRate;

        Debug.Log("Enemy fired a projectile!");
    }
    private void OnDrawGizmos()
{
    if (firePoint != null && player != null)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, player.transform.position);
    }
}

}
