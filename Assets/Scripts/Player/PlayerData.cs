using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private float health = 100;
    [SerializeField] private int level = 1;
    [SerializeField] private float xp = 0;
    private float maxXPPerLevel = 100;
    private float maxPossibleHealth = 100;


    public float GetHealth()
    {
        return health;
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

    public float GetMaxXP()
    {
        return maxXPPerLevel;
    }


    public void AddXP(float xpAmount)
    {
        xp += xpAmount;
        IncreaseLevelIfPossible();
    }


    public void IncreaseLevelIfPossible()
    {
        if (xp >= maxXPPerLevel)
        {
            level += 1;

            float overflowxp = xp - maxXPPerLevel;
            if (overflowxp > 0)
            {
                xp = overflowxp;
            }
            else
            {
                xp = 0;
            }
        }

    }


    public void TakeDamage(float damage)
    {
        if (health >= 0)
        {
            health -= damage;
        }
    }


    
}
