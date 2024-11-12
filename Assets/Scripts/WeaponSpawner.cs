using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{

    [SerializeField] Sprite[] weaponSprites;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomWeaponIndex = Random.Range(0, weaponSprites.Length);

        print(weaponSprites[randomWeaponIndex]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
