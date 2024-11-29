using System.Diagnostics;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private GameObject meleeAttackPrefab;
    private AnimationState playerAnimationState;
    private Vector3 animationPosition = Vector3.zero;
    private Quaternion animationRotation = Quaternion.identity;
    private Vector3 animationScale = Vector3.one;

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

    public void MeleeWeaponAttackPlayerInstatiation(GameObject playerGameObject, Transform playerTransform, AnimationState animationState)
    {


        switch (animationState)
        {
            case AnimationState.player_walk_up:
            case AnimationState.player_idle_up:
                animationRotation = Quaternion.Euler(0, 0, 180);
                animationPosition = new Vector3 (0, 0, 0);
                break;

            case AnimationState.player_walk_left:
            case AnimationState.player_idle_left:
                animationRotation = Quaternion.Euler(0, 0, 270);
                animationPosition = new Vector3(0, 0, 0);
                break;


            case AnimationState.player_walk_right:
            case AnimationState.player_idle_right:
                animationScale = new Vector3(1f, -1f, 0);
                animationPosition = new Vector3(0, 0, 0);
                animationRotation = Quaternion.Euler(0, 0, 90);
                break;


            case AnimationState.player_walk_down:
            case AnimationState.player_idle_down:
                animationPosition = new Vector3(0, 0, 0);
                animationRotation = Quaternion.Euler(0, 0, 0);
                break;

            default:
                animationPosition = new Vector3(0, 0, 0);
                print("DEFAULT");
                break;

        }
        var meleeAttackInstance = Instantiate(meleeAttackPrefab, animationPosition, animationRotation);

        meleeAttackInstance.transform.SetParent(playerTransform);

        meleeAttackInstance.transform.rotation = animationRotation;
        meleeAttackInstance.transform.localScale = animationScale;
        meleeAttackInstance.transform.localPosition = animationPosition;

        
    }
        
    }
