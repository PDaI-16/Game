using UnityEngine;

public class HatFactory
{

    public Hat CreateHat(ItemCategory category, Sprite sprite, float damageMultiplier, float attackSpeedMultiplier)
    {
        // Luo ja palauta Hat-olio
        return new Hat(category, sprite, damageMultiplier, attackSpeedMultiplier);
    }

}
        