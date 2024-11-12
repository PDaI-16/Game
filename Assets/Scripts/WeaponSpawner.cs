using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] Sprite[] weaponSprites;

    [SerializeField] GameObject weapon;

    private SpriteRenderer weaponSpriteRenderer;

    [SerializeField] Sprite chosenSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //weapon = GameObject.Find("Weapon");

        int randomWeaponIndex = Random.Range(0, weaponSprites.Length);
        chosenSprite = weaponSprites[randomWeaponIndex];

        print(weaponSprites[randomWeaponIndex]);

        weaponSpriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
        weaponSpriteRenderer.sprite = weaponSprites[randomWeaponIndex];

        Instantiate(weapon, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
