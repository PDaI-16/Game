using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * References:
 * https://docs.unity3d.com/6000.0/Documentation/Manual/instantiating-prefabs-intro.html
 * https://discussions.unity.com/t/best-way-to-reference-a-prefab-game-object/935673/7
 * https://discussions.unity.com/t/instantiating-gameobjects-at-random-screen-positions/633835
 * https://gamedevbeginner.com/how-to-make-an-inventory-system-in-unity/
 */

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private WeaponSO _weaponSO;
    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private GameObject _background;
    [SerializeField] private WeaponData _defaultWeaponData;
    [SerializeField] private GameObject _inventory;

    private PlayerWeaponController _playerWeaponController;
    private InventoryController _inventoryController;

    private void Start()
    {
        StoreDefaultWeapon();
        SpawnWeapon(5);
        SpawnWeapon(5);
    }

    private void Update()
    {
        // Automatically equip the latest picked weapon
        if (_inventoryController != null && _inventoryController.weaponsInInventory.Count > 0)
        {
            var lastWeapon = _inventoryController.weaponsInInventory[^1]; // ^1 gets the last element
            _playerWeaponController.SetPlayerWeapon(lastWeapon);
        }
    }

    private void StoreDefaultWeapon()
    {
        Debug.Log("Creating default weapon and adding to inventory.");

        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        _playerWeaponController = player.GetComponent<PlayerWeaponController>();
        if (_playerWeaponController == null)
        {
            Debug.LogError("PlayerWeaponController script not found on Player.");
            return;
        }

        _inventoryController = _inventory.GetComponent<InventoryController>();
        if (_inventoryController == null)
        {
            Debug.LogError("InventoryController script not found on Inventory.");
            return;
        }

        _inventoryController.AddWeapon(_defaultWeaponData);
        _playerWeaponController.SetPlayerWeapon(_inventoryController.weaponsInInventory[0]);
    }

    private void SpawnWeapon(int multiplier)
    {
        // Get random stats from WeaponSO
        var (weaponType, randomSprite, damage, attackSpeed, weaponScore) = _weaponSO.GetRandomWeaponStats(multiplier);

        // Generate a random spawn position
        var spawnPosition = GetRandomSpawnPosition();

        // Instantiate the weapon and set its values
        var weaponInstance = Instantiate(_weaponPrefab, spawnPosition, Quaternion.identity);
        var weaponScript = weaponInstance.GetComponent<Weapon>();

        if (weaponScript != null)
        {
            weaponScript.SetValues(weaponType, randomSprite, damage, attackSpeed, weaponScore, false);
        }
        else
        {
            Debug.LogError("Weapon script not found on the weapon prefab.");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (_background == null)
        {
            Debug.LogError("Background object is not assigned.");
            return Vector3.zero;
        }

        var renderer = _background.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer not found on the background object.");
            return Vector3.zero;
        }

        var randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        var randomY = Random.Range(renderer.bounds.min.y, renderer.bounds.max.y);
        return new Vector3(randomX, randomY, 0);
    }
}