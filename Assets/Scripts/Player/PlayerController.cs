using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
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
/*    [SerializeField] GameObject Inventory;
    [SerializeField] InventoryController InventoryController;*/
/*    [SerializeField] List<WeaponData> InventoryWeapons;*/
    [SerializeField] int movementSpeed;

    [SerializeField] GameObject WeaponArm;
    [SerializeField] GameObject WeaponPrefab;

    [SerializeField] public string lookDirection;

    private Vector2 _movementInput;
    private Animator _playerAnimator;
    private Camera _mainCamera;

    private Vector3 _mousePosition;
    private Vector3 _screenPoint;

    private bool _isMoving;
    private AnimationState currentAnimationState;
    private AnimationState newAnimationState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
/*        InventoryController = Inventory.GetComponent<InventoryController>();
        InventoryWeapons = InventoryController.weaponsInInventory;*/
        _mainCamera = Camera.main;
        movementSpeed = 4;
        _playerAnimator = GetComponent<Animator>();
        
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


