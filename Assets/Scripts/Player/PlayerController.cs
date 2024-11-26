using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
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

    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] int movementSpeed;


    private Vector2 _movementInput;
    private Animator _playerAnimator;
    private Camera _mainCamera;

    private Vector3 _mousePosition;
    private Vector3 _screenPoint;

    private bool _isMoving;
    private AnimationState currentAnimationState;
    private AnimationState newAnimationState;

    [SerializeField] GameObject WeaponPrefab;
    [SerializeField] private Weapon currentWeapon = null;
    [SerializeField] private InventoryGO inventoryGOScript;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;


    private GameObject weaponArm;
    private GameObject weaponInstance = null;
    private GameObject currentWeaponObject = null;
    private Weapon previousWeaponData = null;

    [SerializeField] private ItemSpawner itemSpawner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
        movementSpeed = 4;
        _playerAnimator = GetComponent<Animator>();

        weaponArm = GameObject.Find("Weapon Arm");


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


        //Change weapon (just for testing)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Get first weapon from inventory and equip it (JUST FOR TESTING EQUIPPING)
            EquipWeapon(inventoryGOScript.InventoryData.GetWeaponFromInventory(0));
        }

    }

    public void EquipWeapon(Weapon WeaponData)
    {
            // Check if the current weapon is different from the previous one
            if (WeaponData != previousWeaponData)
            {
                // Destroy weapon before new is created
                if (currentWeaponObject != null)
                {
                    Destroy(currentWeaponObject);
                }
                // Spawn the new weapon and assign it to currentWeaponObject
                currentWeaponObject = itemSpawner.SpawnWeapon(WeaponData, weaponArm, new Vector2(0, 0), false);

                // Set previousWeaponObject to the current weapon object for next comparison

                currentWeapon = WeaponData;
                previousWeaponData = WeaponData;


              // Adjust the localPosition from the parent objects player's weaponArm, because otherwise it wont be zero.

                Transform weaponCloneTransform = weaponArm.transform.Find("Weapon(Clone)");

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
    }


}


