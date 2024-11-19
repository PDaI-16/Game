using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public enum AnimationState
{
    IdleUp, // 0 , AnimationState integer 
    IdleRight, // 1
    IdleDown, // 2
    IdleLeft, // 3
    WalkUp, // 4
    WalkRight, // 5
    WalkDown, // 6
    WalkLeft // 7
}


public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] GameObject Inventory;
    [SerializeField] InventoryController InventoryController;
    [SerializeField] List<WeaponData> InventoryWeapons;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryController = Inventory.GetComponent<InventoryController>();
        InventoryWeapons = InventoryController.weaponsInInventory;
        _mainCamera = Camera.main;
        movementSpeed = 2;
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

        /*        FlipPlayer();*/

        /*        AnimationHandler();*/

        UpdateIsMoving();
        UpdateLookDirection();
        UpdateAnimationState();

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
            print("UP");
            currentAnimationState = _isMoving ? AnimationState.WalkUp : AnimationState.IdleUp;
        }
        else if(angleInDegrees > 45 && angleInDegrees < 135)
        {
            print("RIGHT");
            currentAnimationState = _isMoving ? AnimationState.WalkRight : AnimationState.IdleRight;
        }
        else if(angleInDegrees < -45 && angleInDegrees > -135)
        {
            print("LEFT");
            currentAnimationState = _isMoving ? AnimationState.WalkLeft : AnimationState.IdleLeft;
        }
        else if(angleInDegrees > 135 || angleInDegrees < -135)
        {
            print("DOWN");
            currentAnimationState = _isMoving ? AnimationState.WalkDown : AnimationState.IdleDown;
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

    private void UpdateAnimationState()
    {
        if (_playerAnimator != null)
        {
            // Reset all triggers
            _playerAnimator.SetInteger("AnimationState", (int)currentAnimationState);
            print("Current Animation state: " + currentAnimationState);

            _playerAnimator.ResetTrigger("TriggerIdleUp");
            _playerAnimator.ResetTrigger("TriggerIdleRight");
            _playerAnimator.ResetTrigger("TriggerIdleDown");
            _playerAnimator.ResetTrigger("TriggerIdleLeft");
            _playerAnimator.ResetTrigger("TriggerWalkUp");
            _playerAnimator.ResetTrigger("TriggerWalkRight");
            _playerAnimator.ResetTrigger("TriggerWalkDown");
            _playerAnimator.ResetTrigger("TriggerWalkLeft");

            // Set the trigger for the current animation state
            switch (currentAnimationState)
            {
                case AnimationState.IdleUp:
                    _playerAnimator.SetTrigger("TriggerIdleUp");
                    
                    break;
                case AnimationState.IdleRight:
                    _playerAnimator.SetTrigger("TriggerIdleRight");
                    break;
                case AnimationState.IdleDown:
                    _playerAnimator.SetTrigger("TriggerIdleDown");
                    break;
                case AnimationState.IdleLeft:
                    _playerAnimator.SetTrigger("TriggerIdleLeft");
                    break;
                case AnimationState.WalkUp:
                    _playerAnimator.SetTrigger("TriggerWalkUp");
                    break;
                case AnimationState.WalkRight:
                    _playerAnimator.SetTrigger("TriggerWalkRight");
                    break;
                case AnimationState.WalkDown:
                    _playerAnimator.SetTrigger("TriggerWalkDown");
                    break;
                case AnimationState.WalkLeft:
                    _playerAnimator.SetTrigger("TriggerWalkLeft");
                    break;
            }
        }
    }


    void AnimationHandler()
    {
        Animator animator = GetComponent<Animator>();


        if (_movementInput.x == 0 && _movementInput.y == 0)
        {
            _playerAnimator.SetInteger("walkingState", 0); // Idle
        }
        else if(_movementInput.x != 0)
        {
            _playerAnimator.SetInteger("walkingState", 1); // Walk right or left
        }
        else if(_movementInput.y > 0 && _movementInput.x == 0)
        {
            _playerAnimator.SetInteger("walkingState", 2); // Walk up
        }
        else if(_movementInput.y < 0 && _movementInput.x == 0)
        {
            _playerAnimator.SetInteger("walkingState", 3); // Walk down
        }

    }

    void FlipPlayer()
    {

        if (_mousePosition.x < _screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }            
    }
}


