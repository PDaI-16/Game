using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    private Sprite sprite;
    public float attackSpeed;
    public float weaponScore;

    private int maxPossibleDamage;
    private int maxPossibleAttackSpeed;
    private int baseDamage = 3;
    private int baseAttackSpeed = 3;

    public void Initialize(Sprite weaponSprite, int multiplier)
    {

        GetComponentInChildren<SpriteRenderer>().sprite = weaponSprite;

        maxPossibleDamage = baseDamage * multiplier;
        maxPossibleAttackSpeed = baseAttackSpeed * multiplier;

        damage = Random.Range(1, maxPossibleDamage);
        attackSpeed = Random.Range(1, maxPossibleAttackSpeed);

        weaponScore = damage * attackSpeed;
    }


}
