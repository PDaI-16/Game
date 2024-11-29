using System.Diagnostics;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private GameObject meleeAttackPrefab;
    private AnimationState playerAnimationState;
    private Vector3 animationPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Static so no need to create this object to use.

    // Needed : Player sprite location and animationstate the player is currently in
    // ability to make current weapon dissappear during the slash animation
    // Differntiate who performed the function to know which objects can be hit (MAYBE IF SAME KIND OF FUNCTION IS USED BY ENEMIES TOO)

    public void MeleeWeaponAttackPlayerInstatiation(GameObject PlayerGameObject, Transform playerTransform, AnimationState animationState)
    {


        switch (animationState)
        {
            case AnimationState.player_walk_up:
            case AnimationState.player_idle_up:
                animationPosition = new Vector3 (0, 0, 0);
                break;

            case AnimationState.player_walk_left:
            case AnimationState.player_idle_left:
                animationPosition = new Vector3(0, 0, 0);
                break;


            case AnimationState.player_walk_right:
            case AnimationState.player_idle_right:
                animationPosition = new Vector3(0, 0, 0);
                break;


            case AnimationState.player_walk_down:
            case AnimationState.player_idle_down:
                animationPosition = new Vector3(0, 0, 0);
                break;

            default:
                animationPosition = new Vector3(0, 0, 0);
                print("DEFAULT");
                break;

        }
        var meleeAttackInstance = Instantiate(meleeAttackPrefab, playerTransform.position, Quaternion.identity);

        meleeAttackInstance.transform.localRotation = Quaternion.identity;
        meleeAttackInstance.transform.localScale = Vector3.one;

        
    }
        
    }
