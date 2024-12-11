using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public enum AnimationState
{
    player_idle_up, 
    player_idle_right, 
    player_idle_down,  
    player_idle_left, 
    player_walk_up, 
    player_walk_right, 
    player_walk_down, 
    player_walk_left 
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerData playerData;

    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] int movementSpeed;
    [SerializeField] private ProjectileAttackGO projectileAttackGO;


    private Vector2 _movementInput;
    private Animator _playerAnimator;
    private Camera _mainCamera;

    private Vector3 _mousePosition;
    private Vector3 _screenPoint;

    private bool _isMoving;
    public AnimationState currentAnimationState;
    public AnimationState newAnimationState;

    [SerializeField] GameObject WeaponPrefab;
    
    [SerializeField] private InventoryGO inventoryGOScript;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;


    [SerializeField] private Camera currentCamera;


    private GameObject weaponArm;

    private GameObject rangedArm;
    private Transform rangedArmTransform;

    private SortingGroup weaponArmSortingGroup;
    private SortingGroup rangedArmSortingGroup;

    private GameObject weaponArmMelee;
    private GameObject weaponArmMagic;

    private GameObject currentWeaponObject = null;
    private Weapon currentWeaponData = null;
    private Weapon previousWeaponData = null;

    private Vector3 defaultPositionRangedArm;
    private Vector3 newRangedWeaponPosition;


    [SerializeField] private ItemSpawner itemSpawner;


    // Attack related stuff

    [SerializeField] private GameObject meleeAttack;
    [SerializeField] private MeleeAttackGO meleeAttackGOScript;
    [SerializeField] private BoxCollider2D meleeAttackHitbox;

    [SerializeField] private GameObject deathScreenPanel;

/*    [SerializeField] private MainMenu mainMenu;*/


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerData = new PlayerData(MainMenu.playerClass);


        _mainCamera = Camera.main;
        movementSpeed = 4;
        _playerAnimator = GetComponent<Animator>();

        weaponArm = GameObject.Find("Weapon Arm");
        rangedArm = GameObject.Find("Ranged Arm");
        weaponArmMelee = GameObject.Find("Melee");
        weaponArmMagic = GameObject.Find("Magic");


        rangedArmTransform = rangedArm.transform;

        weaponArmSortingGroup = weaponArm.GetComponent<SortingGroup>();
        rangedArmSortingGroup = rangedArm.GetComponent<SortingGroup>();

        meleeAttackHitbox.gameObject.SetActive(false);

    } // Update is called once per frame



    void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        _movementInput.Normalize(); // Normalizing the movement input so that diagonal movement is not faster than others.

        playerRigidbody.linearVelocity = _movementInput * movementSpeed;

        _mousePosition = Input.mousePosition;
        _screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);


        UpdateIsMoving();
        UpdateLookDirection();
        ChangeAnimationState(newAnimationState);

        if (currentWeaponData != null)
        {
            if (currentWeaponData.Category == ItemCategory.Ranged)
            {
                ObjectRotateAccordingToMouse.RotateObjectForRangedWeapon(rangedArm.transform, currentCamera);
                ChangeRangedWeaponPositionBasedOnAnimation(newAnimationState);
            }
        }



        PlayerInputs();
        CheckIfShouldDie();

    }

    public void CheckIfShouldDie()
    {
        if (playerData.GetHealth() <= 0.0f)
        {
            deathScreenPanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void PlayerInputs()
    {
        //Change to latest weapon in the inventory (just for testing before proper inventory is made...)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int weaponCountFromInventory = inventoryGOScript.InventoryData.GetItemTypeCountFromInventory(ItemType.Weapon);
            EquipWeapon(inventoryGOScript.InventoryData.GetWeaponFromInventory(weaponCountFromInventory - 1));
        }

        PlayerAttack();
    }

    void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button (M1)
        {
            if (currentWeaponData != null)
            {
                switch (currentWeaponData.Category)
                {
                    case ItemCategory.Melee:
                        if (meleeAttackGOScript != null && meleeAttackHitbox != null)
                        {
                            try
                            {
                                meleeAttackHitbox.gameObject.SetActive(true);
                                meleeAttackGOScript.Attack(currentWeaponData, currentAnimationState, currentWeaponObject, currentCamera);
                            }
                            catch
                            {
                                Debug.LogWarning("Failed to find attack script before activating the script");
                            }


                        }
                        else
                        {
                            Debug.LogWarning("Melee attack go script or hitbox not found by PlayerAttack");
                        }
                        break;

                    case ItemCategory.Magic:
                    case ItemCategory.Ranged:
                        
                        if (projectileAttackGO != null)
                        {
                            projectileAttackGO.ProjectileAttack(currentWeaponData.Category, currentWeaponData.AttackSpeed, currentCamera);
                        }

                        else
                        {
                            Debug.LogError("ProjectileAttackGo script not found by playerController");
                        }

                        break;
                }
            }
            else
            {
                Debug.Log("Do weapon equiped - PlayerAttack");
            }


            
        }
    }

    public Weapon GetCurrentWeaponData()
    {
        return currentWeaponData;
    }


    public void EquipWeapon(Weapon WeaponData)
    {
        // Check if the current weapon is different from the previous one
        if (WeaponData != previousWeaponData)
        {
            // Destroy the previous weapon before spawning a new one
            if (currentWeaponObject != null)
            {
                Destroy(currentWeaponObject);
            }

            GameObject chosenArm = null;

            // Determine the appropriate arm based on weapon category
            switch (WeaponData.Category)
            {
                case ItemCategory.Melee:
                    Debug.Log("Equipping a melee weapon.");
                    // Add logic for melee weapon handling
                    chosenArm = weaponArmMelee;
                    break;

                case ItemCategory.Ranged:
                    Debug.Log("Equipping a ranged weapon.");
                    // Add logic for ranged weapon handling
                    chosenArm = rangedArm;
                    break;

                case ItemCategory.Magic:
                    Debug.Log("Equipping a magic weapon.");
                    // Add logic for magic weapon handling
                    chosenArm = weaponArmMagic;
                    break;

                default:
                    Debug.LogWarning("Unknown weapon category.");
                    // Handle cases where the category is not recognized
                    break;
            }

            // Spawn the new weapon and assign it to currentWeaponObject
            if (chosenArm != null)
            {
                currentWeaponObject = itemSpawner.SpawnWeapon(WeaponData, chosenArm, new Vector2(0, 0), false);
                Debug.Log("Weapon was spawned to arm");
            }
            else
            {
                Debug.LogError("Specific arm was not found.");
            }

            // Set the current and previous weapon data for comparison
            currentWeaponData = WeaponData;
            previousWeaponData = WeaponData;

            // Adjust the localPosition of the weapon from the parent object's player's weaponArm
            Transform weaponCloneTransform = chosenArm.transform.Find("Weapon(Clone)");

            if (weaponCloneTransform != null)
            {
                // Set its localPosition to (0, 0, 0)
                weaponCloneTransform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogError("Weapon(Clone) not found as a child of weaponArm.");
            }
        }
        else
        {
            Debug.Log("Weapon is the same as the previous one, not spawning a new one.");
        }
    }

    void UpdateLookDirection()
    {
        Vector2 dir = _mousePosition - _screenPoint;

        // Calculate angle in radians
        float angleInRadians = Mathf.Atan2(dir.x, dir.y);

        // Convert radians to degrees
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
/*
        print("Mouse angle in degrees: " + angleInDegrees);*/


        if(angleInDegrees > -45 && angleInDegrees < 45){
            //print("UP");
            newAnimationState = _isMoving ? AnimationState.player_walk_up : AnimationState.player_idle_up;
        }
        else if(angleInDegrees > 45 && angleInDegrees < 135)
        {
            //print("RIGHT");
            newAnimationState = _isMoving ? AnimationState.player_walk_right : AnimationState.player_idle_right;
        }
        else if(angleInDegrees < -45 && angleInDegrees > -135)
        {
            //print("LEFT");
            newAnimationState = _isMoving ? AnimationState.player_walk_left : AnimationState.player_idle_left;
        }
        else if(angleInDegrees > 135 || angleInDegrees < -135)
        {
            //print("DOWN");
            newAnimationState = _isMoving ? AnimationState.player_walk_down : AnimationState.player_idle_down;
        }
    }


    void ChangeRangedWeaponPositionBasedOnAnimation(AnimationState animationState)
    {
        if(rangedArmTransform != null)
        {
            switch (animationState)
            {
                case AnimationState.player_walk_up:
                case AnimationState.player_idle_up:
                    newRangedWeaponPosition = new Vector3(0, 0.15f, 0);
                    break;

                case AnimationState.player_walk_left:
                case AnimationState.player_idle_left:
                    newRangedWeaponPosition = new Vector3(-0.45f, 0, 0);
                    break;


                case AnimationState.player_walk_right:
                case AnimationState.player_idle_right:
                    newRangedWeaponPosition = new Vector3 (0.45f, 0, 0);
                    break;


                case AnimationState.player_walk_down:
                case AnimationState.player_idle_down:
                    newRangedWeaponPosition = defaultPositionRangedArm;
                    break;


                default:
                    newRangedWeaponPosition = defaultPositionRangedArm;
                    break;
            }

            
            rangedArmTransform.transform.localPosition = newRangedWeaponPosition;
        }
        else
        {
            Debug.LogError("Ranged arm transform was null");
            return;
        }

    }


    /// <summary>
    /// Used for weapon arm layer changes depending on the animationstate
    /// </summary>
    /// <param name="animatioState">Current animation state the player is in.</param>

    void UpdateWeaponArmSorting(AnimationState animationState)
    {
        if (weaponArmSortingGroup == null)
        {
            Debug.LogWarning("No weapon arm Sorting Group found!");
            return;
        }

        // Adjust sorting layer and order based on animation state
        switch (animationState)
        {
            case AnimationState.player_walk_up:
            case AnimationState.player_idle_up:
            case AnimationState.player_walk_left:
            case AnimationState.player_idle_left:
                weaponArmSortingGroup.sortingLayerName = "PlayerWeaponBehind";
                rangedArmSortingGroup.sortingLayerName = "PlayerWeaponBehind";
                break;

            case AnimationState.player_walk_down:
            case AnimationState.player_idle_down:
            case AnimationState.player_walk_right:
            case AnimationState.player_idle_right:
                weaponArmSortingGroup.sortingLayerName = "PlayerWeapon"; // Weapon in front of the player
                rangedArmSortingGroup.sortingLayerName = "PlayerWeapon";
                break;

    

            default:
                weaponArmSortingGroup.sortingLayerName = "PlayerWeapon"; // Default sorting order
                rangedArmSortingGroup.sortingLayerName = "PlayerWeapon";
                break;
        }
    }

    public void UpdateIsMoving()
    {
        if(_movementInput.x == 0 && _movementInput.y == 0)
        {
            _isMoving = false;
        }
        else
        {
            _isMoving = true;
        }
    }

    void ChangeAnimationState(AnimationState newState)
    {
        string state = newState.ToString();

        if (currentAnimationState == newState) return;

        _playerAnimator.Play(state);

        currentAnimationState = newState;
        UpdateWeaponArmSorting(newState);
    }


}


