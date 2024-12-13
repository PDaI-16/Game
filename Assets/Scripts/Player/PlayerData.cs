using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private float health = 100;
    [SerializeField] private float defence = 0;

    [SerializeField] private int level = 1;
    [SerializeField] private float xp = 0;
    [SerializeField] private int skillPoints = 1;
    [SerializeField] private ItemCategory initialPlayerClass;

    private float xpRequiredForLevelUp = 100;
    private float requiredXPPerLevelMultiplier = 1.05f;

    private float maxPossibleHealth = 100;

    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private GameObject damageIndicatorPrefab; // Reference to the damage indicator prefab


    private float accumulatedDamage = 0f; // Variable to accumulate damage
    private float damageTimer = 0f; // Timer to track time between damages
    private float damageThresholdTime = 1f; // Time window for accumulating damage (1 second)
    private int damageCount = 0; // Counter for number of damages

    public PlayerData(ItemCategory initialClass, Transform healthTextTransform, GameObject damageIndicator) // InitialClass from mainmenu
    {
        this.initialPlayerClass = initialClass;
        this.playerTransform = healthTextTransform;
        this.damageIndicatorPrefab = damageIndicator;
    }

    public void SetInitialClass(ItemCategory chosenClass)
    {
        initialPlayerClass = chosenClass;
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }

    public void SpendSkillPoints(int cost)
    {
        if (cost <= skillPoints)
        {
            skillPoints -= cost;
        }
        else
        {
            Debug.Log("Not enough skillpoints");
        }

    }

    public ItemCategory GetInitialPlayerClassFromPlayer()
    {
        return initialPlayerClass;
    }

    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float newHealth)
    {
        health = Mathf.Max(0, newHealth); // Ensure health doesn't go below 0
    }

    public void AddHealth(float amount)
    {
        // Increase the player's maxHealth and heal the player by the same amount
        float newMaxHealth = maxPossibleHealth + amount;
        maxPossibleHealth = newMaxHealth;
        health += amount;
        health = Mathf.Min(health, maxPossibleHealth);
        health = Mathf.Max(0, health);
    }
    public float GetDefence()
    {
        return defence;
    }

    public void SetDefence(float newDefence)
    {
        defence = Mathf.Max(0, newDefence); // Ensure defence doesn't go below 0
    }

    public float GetXP()
    {
        return xp;
    }

    public float GetMaxPossibleHealth()
    {
        return maxPossibleHealth;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetXpRequiredForLevelUp()
    {
        return xpRequiredForLevelUp;
    }


    public void AddXP(float xpAmount)
    {
        xp += xpAmount;
        IncreaseLevelIfPossible();
    }


    public void IncreaseLevelIfPossible()
    {
        if (xp >= xpRequiredForLevelUp)
        {
            level += 1;

            skillPoints += 1;

            // Adding the remaining xp to new level's xp
            float overflowxp = xp - xpRequiredForLevelUp;
            if (overflowxp > 0)
            {
                xp = overflowxp;
            }
            else
            {
                xp = 0;
            }

            // Increase the required xp before level up with multiplier
            xpRequiredForLevelUp = requiredXPPerLevelMultiplier * xpRequiredForLevelUp;
        }

    }


    public void TakeDamage(float damage)
    {
        if (health > 0f)
        {
            // Calculate damage reduction based on defense as a percentage
            float damageAfterDefense = damage * (1 - (defence / 100f));
            damageAfterDefense = Mathf.Max(damageAfterDefense, 0f); // Ensure damage isn't negative

            // Accumulate damage
            accumulatedDamage += damageAfterDefense;
            damageCount++;

            // Subtract damage from health
            health -= damageAfterDefense;
            Debug.Log($"Player took {damageAfterDefense} damage. Health remaining: {health}");

            // Check if the accumulated damage exceeds 10 or the time window has passed
            damageTimer += Time.deltaTime;

            if (damageCount >= 10 || damageTimer >= damageThresholdTime)
            {
                // Spawn the damage indicator with the total accumulated damage
                if (playerTransform != null && damageIndicatorPrefab != null)
                {
                    GameObject damageIndicator = Object.Instantiate(
                        damageIndicatorPrefab,
                        playerTransform.position + new Vector3(0, 1.5f, 0), // Adjust the position if necessary
                        Quaternion.identity
                    );

                    damageIndicator.transform.localScale = new Vector3(1.5f,1.5f,0);
                    damageIndicator.GetComponentInChildren<Canvas>().sortingLayerName = "InventoryUI";
                    damageIndicator.GetComponentInChildren<Canvas>().sortingOrder = 5;

                    // Set the accumulated damage text
                    DamageIndicatorGO damageIndicatorScript = damageIndicator.GetComponent<DamageIndicatorGO>();
                    if (damageIndicatorScript != null)
                    {
                        damageIndicatorScript.SetDamageText(accumulatedDamage);
                    }
                }

                // Reset the damage accumulation
                accumulatedDamage = 0f;
                damageCount = 0;
                damageTimer = 0f;
            }
        }
    }
}
