using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public bool isBoss;  // Flag to indicate if the enemy is a boss
    public float health;
    public float maxHealth;
    public float Damage;
    public int experienceReward = 100;

    private PlayerData playerData = null;

    [SerializeField] private EnemyHealthBar enemyHealthBar;

    [SerializeField] private AIMove aiMovement;
    [SerializeField] private RangedMovement rangedMovement;
    [SerializeField] private BossMovement bossMovement;

    // Amount by which aggro range increases when taking damage
    [SerializeField] private float aggroIncreaseAmount = 10.0f;

    void Start()
    {
        health = maxHealth;

        playerData = GameObject.Find("Player").GetComponent<PlayerController>().playerData;
    }

    public void Update()
    {
        if (playerData == null)
        {
            playerData = GameObject.Find("Player").GetComponent<PlayerController>().playerData;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage. Current health: {health}");
        IncreaseAggroRange();

        CheckDeath();
        enemyHealthBar.UpdateHealthBar();
    }

    private void IncreaseAggroRange()
    {
        if (aiMovement != null)
        {
            aiMovement.aggrostart = aggroIncreaseAmount;
            aiMovement.aggroend = aggroIncreaseAmount;
            Debug.Log($"Aggro range increased. New aggro start: {aiMovement.aggrostart}, aggro end: {aiMovement.aggroend}");
        }
        if (rangedMovement != null){
            rangedMovement.aggroStartDistance = aggroIncreaseAmount;
            rangedMovement.aggroEndDistance = aggroIncreaseAmount;
        }
        if (bossMovement != null){
            bossMovement.aggroStartDistance = aggroIncreaseAmount;
            bossMovement.aggroEndDistance =  aggroIncreaseAmount;
        }
        else
        {
            Debug.LogWarning("AI Movement script not assigned or missing.");
        }
    }

    public void CheckDeath()
    {
        if (health <= 0)
        {
            if (playerData != null)
            {
                playerData.AddXP(experienceReward);
            }
            else
            {
                Debug.LogError("playerData is null, can't award XP.");
            }
            Destroy(transform.root.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerData = other.gameObject.GetComponent<PlayerController>().playerData;
            if (playerData != null)
            {
                playerData.TakeDamage(Damage * Time.deltaTime); // Damage per second
            }
            else
            {
                Debug.LogWarning("PlayerData script not found on Player.");
            }
        }
    }
}
