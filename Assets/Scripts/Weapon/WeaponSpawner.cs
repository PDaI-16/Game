using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/*https://docs.unity3d.com/6000.0/Documentation/Manual/instantiating-prefabs-intro.html
https://discussions.unity.com/t/best-way-to-reference-a-prefab-game-object/935673/7
https://discussions.unity.com/t/instantiating-gameobjects-at-random-screen-positions/633835 
https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/
 */

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject background;

    public void Start()
    {
        SpawnWeapon(5);
    }

    public void SpawnWeapon(int multiplier)
    {
        Sprite randomSprite = weaponSO.PossibleSprites[Random.Range(0, weaponSO.PossibleSprites.Length)];

        // Generate a random position within the background bounds
        Renderer renderer = background.GetComponent<Renderer>();
        float randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomY = Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);



        float damage = Random.Range(1.0f, weaponSO.BaseDamage*multiplier);
        float attackSpeed = Random.Range(1.0f, weaponSO.BaseAttackSpeed*multiplier);
        float weaponScore = damage * attackSpeed;



        GameObject weaponInstance = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);

        Weapon weaponScript = weaponInstance.GetComponent<Weapon>();

        if (weaponScript != null)
        {
            weaponScript.SetValues(randomSprite, damage, attackSpeed, weaponScore);
        }
        else
        {
            Debug.LogError("Weapon script not found on the weapon prefab.");
        }


    }

}