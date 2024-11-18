using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite _sprite;
    public float _damage;
    public float _attackSpeed;
    public float _weaponScore;

    [SerializeField] public WeaponData WeaponData;

    public void SetValues(Sprite sprite, float damage, float attackspeed, float weaponscore)
    {
        _sprite = sprite;
        _damage = damage;
        _attackSpeed = attackspeed;
        _weaponScore = weaponscore;

        SpriteRenderer spriterenderer = GetComponentInChildren<SpriteRenderer>();
        spriterenderer.sprite = _sprite;

        WeaponData = new WeaponData(_sprite, _damage, _attackSpeed, _weaponScore);
    }

    public (Sprite, float, float, float) GetValues()
    {
        
        return (_sprite, _damage, _attackSpeed, _weaponScore);
    }

}
