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
    [SerializeField] private Sprite[] weaponSprites; // Array of weapon sprites
    [SerializeField] private GameObject weaponPrefab; // Prefab for the weapon to spawn
    [SerializeField] private GameObject background; // Background object to get bounds

    [SerializeField] private int multiplier = 2; // Multiplier for weapon stats

    private void Start()
    {
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        // Choose a random sprite from the weapon sprites array
        int randomWeaponIndex = Random.Range(0, weaponSprites.Length);
        Sprite chosenSprite = weaponSprites[randomWeaponIndex];

        // Generate a random position within the background bounds
        Renderer renderer = background.GetComponent<Renderer>();
        float randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomY = Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

        // Instantiate the weapon prefab at the random position
        GameObject weaponInstance = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);

        // Initialize the weapon with the chosen sprite and stat multiplier
        Weapon weaponComponent = weaponInstance.GetComponent<Weapon>();
        weaponComponent.Initialize(chosenSprite, multiplier);
    }
}