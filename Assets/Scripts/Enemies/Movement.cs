using UnityEngine;

public class AIMove : MonoBehaviour
{
    public float speed;                // Speed of the enemy
    public float aggrostart;           // Aggro start distance
    public float aggroend;             // Aggro end distance

    private GameObject player;         // Player reference (will be accessed via Singleton)
    private float distance;            // Distance between player and enemy
    private bool isAggroed;            // Whether the enemy is in aggro state

    private void Start()
    {
        // Access the player reference from the singleton
        player = PlayerReference.Instance;

        // Ensure the player exists (i.e., the player singleton is set up correctly)
        if (player == null)
        {
            Debug.LogError("Player reference not found! Ensure the PlayerReference script is attached to the player object.");
        }
    }

    private void Update()
    {
        // Only proceed if the player is assigned
        if (player != null)
        {
            // Calculate the distance between the enemy and the player
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Start aggro if the enemy is within aggro start distance
            if (!isAggroed && distance < aggrostart)
            {
                isAggroed = true;
            }
            // Stop aggro if the enemy moves outside the aggro end distance
            else if (isAggroed && distance > aggroend)
            {
                isAggroed = false;
            }

            // If aggroed, move towards the player
            if (isAggroed)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            // If no player is assigned, log a message
            Debug.Log("Player does not exist or PlayerReference is missing.");
        }
    }
}
