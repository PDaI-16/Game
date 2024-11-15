using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite _sprite;
    public float _damage;
    public float _attackSpeed;
    public float _weaponScore;


    public void SetValues(Sprite sprite, float damage, float attackspeed, float weaponscore)
    {
        _sprite = sprite;
        _damage = damage;
        _attackSpeed = attackspeed;
        _weaponScore = weaponscore;

        SpriteRenderer spriterenderer = GetComponentInChildren<SpriteRenderer>();
        spriterenderer.sprite = _sprite;
    }


}
