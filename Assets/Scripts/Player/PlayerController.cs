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


    [SerializeField] public GameObject weaponArm;
    [SerializeField] private Transform weaponArmTransform;
    GameObject weaponInstance = null;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
        movementSpeed = 4;
        _playerAnimator = GetComponent<Animator>();

        weaponArm = GameObject.Find("Weapon Arm");
        weaponArmTransform = weaponArm.transform;

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
            // Get the weapon data from inventory
            currentWeapon = inventoryGOScript.InventoryData.GetWeaponFromInventory(1);

            if (weaponArmTransform == null)
            {
                Debug.LogError("Weapon Arm not assigned!");
                return;
            }

            // Instantiate the weapon prefab at the position of the WeaponArm
            // Using Quaternion.identity ensures no initial rotation
            if (weaponInstance == null)
            {
                weaponInstance = Instantiate(WeaponPrefab, weaponArmTransform.position, Quaternion.identity);

                // Parent the instantiated weapon to the weapon arm
                weaponInstance.transform.SetParent(weaponArmTransform);

                // Force the weapon to stay at position (0, 0, 0) relative to the weapon arm
                weaponInstance.transform.localPosition = Vector3.zero;

                // Ensure it follows the rotation of the weapon arm (fixes the issue with initial rotation)
                weaponInstance.transform.localRotation = Quaternion.identity;

                // Optional: Adjust scale if needed (ensuring it doesn't scale unexpectedly)
                weaponInstance.transform.localScale = Vector3.one;

                // Access the WeaponGO script attached to the weapon instance
                WeaponGO weaponGO = weaponInstance.GetComponent<WeaponGO>();

                if (weaponGO != null)
                {
                    // Initialize the weapon or set any data you need
                    weaponGO.Initialize(currentWeapon, false);  // Assuming you have an Initialize method in WeaponGO
                    Debug.Log("Weapon equipped successfully and initialized!");
                }
                else
                {
                    Debug.LogError("WeaponGO script not found on weapon prefab!");
                }

                // Debug log to confirm the weapon is equipped
                Debug.Log("Weapon equipped successfully!");
            }


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


