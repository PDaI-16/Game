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
        SpawnWeapon(5);
    }

    public void SpawnWeapon(int multiplier)
    {
        // Get all random stats from WeaponSO
        var (randomSprite, damage, attackSpeed, weaponScore) = weaponSO.GetRandomWeaponStats(multiplier);

        // Generate position
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Instantiate the weapon and set its values
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

    private Vector3 GetRandomSpawnPosition()
    {
        Renderer renderer = background.GetComponent<Renderer>();
        float randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomY = Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        return new Vector3(randomX, randomY, 0);
    }
}