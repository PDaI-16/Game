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
    [SerializeField] public WeaponData defaultWeaponData;
    [SerializeField] public GameObject Inventory;
    [SerializeField] public GameObject WeaponPrefab;
    [SerializeField] public GameObject WeaponArm;
    [SerializeField] private WeaponData weaponData;

    [SerializeField] public GameObject Player;
    [SerializeField] private PlayerWeaponController playerWeaponController;

    [SerializeField] public InventoryController inventoryController;

    public void Start()
    {
        StoreDefaultWeapon();
/*        EquipDefaultWeapon();*/
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
            weaponScript.SetValues(randomSprite, damage, attackSpeed, weaponScore, false);
        }
        else
        {
            Debug.LogError("Weapon script not found on the weapon prefab.");
        }
    }


    private void StoreDefaultWeapon()
    {
        /*        defaultWeaponData = new WeaponData();*/
        print("Create default weapon to inventory");

        Player = GameObject.FindWithTag("Player");
        playerWeaponController = Player.GetComponent<PlayerWeaponController>();

        inventoryController = Inventory.GetComponent<InventoryController>();
        if (inventoryController != null)
        {
            inventoryController.AddWeapon(defaultWeaponData);
        }

        playerWeaponController.SetPlayerWeapon(inventoryController.weaponsInInventory[0]);
       

    }

    private void Update()
    {  
        // Just for testing, changes to latest picked weapon automatically...
        if (inventoryController.weaponsInInventory.Count > 0) // Check if there is at least one weapon
        {
            playerWeaponController.SetPlayerWeapon(
                inventoryController.weaponsInInventory[inventoryController.weaponsInInventory.Count - 1]
            );
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