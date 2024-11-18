using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    [SerializeField] private WeaponData _weaponData;
    private SpriteRenderer _weaponSpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        _weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    public void SetPlayerWeapon(WeaponData weapondata)
    {
        _weaponData = weapondata;

        if (_weaponSpriteRenderer != null)
        {
            _weaponSpriteRenderer.sprite = weapondata.Sprite;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
