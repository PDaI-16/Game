using UnityEngine;

public class RangedMovement : MonoBehaviour
{
    public GameObject player;         // Reference to the player GameObject
    public float speed;               // Movement speed of the enemy
    public float aggroStartDistance;  // Distance at which the enemy becomes aggroed
    public float aggroEndDistance;    // Distance at which the enemy loses aggro
    public float attackDistance;      // Ideal distance for ranged attacks
    public float attackGraceTime;     // Time the enemy waits after exiting attack range

    private float distanceToPlayer;   // Current distance to the player
    private bool isAggroed;           // Tracks if the enemy is aggroed
    public bool isInGracePeriod;     // Tracks if the enemy is in the grace period
    private float graceTimer;         // Tracks time left in the grace period

    private void Update()
    {
        if (player != null)
        {
            // Calculate the distance to the player
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // Check if the enemy should become aggroed or lose aggro
            if (!isAggroed && distanceToPlayer < aggroStartDistance)
            {
                isAggroed = true;
            }
            else if (isAggroed && distanceToPlayer > aggroEndDistance)
            {
                isAggroed = false;
            }

            // Handle movement logic
            if (isAggroed)
            {
                if (distanceToPlayer > attackDistance && !isInGracePeriod)
                {
                    // Move towards the player until reaching the attack distance
                    Vector2 direction = (player.transform.position - transform.position).normalized;
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
                else if (distanceToPlayer <= attackDistance)
                {
                    // Enemy is in attack range, start the grace period
                    isInGracePeriod = true;
                    graceTimer = attackGraceTime;
                }
            }

            // Handle grace period countdown
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
}
