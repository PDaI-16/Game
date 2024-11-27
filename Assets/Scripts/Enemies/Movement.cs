using UnityEngine;

public class AIMove : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float aggrostart;
    public float aggroend;

    private float distance;
    private bool isAggroed;

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (!isAggroed && distance < aggrostart)
        {
            isAggroed = true;
        }
        else if (isAggroed && distance > aggroend)
        {
            isAggroed = false;
        }

        if (isAggroed)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
