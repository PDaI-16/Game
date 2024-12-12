using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MeleeAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Animator meleeAttackAnimator;
    private GameObject currentWeaponGameObject;
    [SerializeField] private Camera currentCamera;
    

    [SerializeField] private AnimationState previousState;
    private Transform parentTransform;

    private float damage = 0; 

    void Start()
    {


    }

    private void Update()
    {
        if (transform != null && parentTransform != null)
        {
            transform.position = parentTransform.position;
        } 
        
    }

    public void Attack(float totalDamage, AnimationState playerAnimationState, GameObject currentWeaponObject, Camera camera, Transform playerTransform)
    {
        currentCamera = camera;
        currentWeaponGameObject = currentWeaponObject;
        currentWeaponObject.SetActive(false);

        damage = totalDamage;

        parentTransform = playerTransform;

        FlipMeleeAttack(playerAnimationState);
        ObjectRotateAccordingToMouse.RotateObjectForMeleeAttack(gameObject.transform, currentCamera);

        meleeAttackAnimator.Play("Melee attack");

    }

    private void FlipMeleeAttack(AnimationState playerAnimationState)
    {
        if (playerAnimationState == previousState)
        {
            return;
        }

        transform.localScale = new Vector3(1f, 1f, 0);

        previousState = playerAnimationState;

        switch (playerAnimationState)
        {
            case AnimationState.player_walk_up:
            case AnimationState.player_idle_up:
                break;

            case AnimationState.player_walk_left:
            case AnimationState.player_idle_left:
                break;


            case AnimationState.player_walk_right:
            case AnimationState.player_idle_right:
                transform.localScale = new Vector3(-1f, 1f, 0);
                break;


            case AnimationState.player_walk_down:
            case AnimationState.player_idle_down:
                break;

            default:
                break;

        }
        ObjectRotateAccordingToMouse.RotateObjectForRangedWeapon(gameObject.transform, currentCamera);
    }

    // Activates on after animation event.
    void DeactivateHitbox()
    {
        if (currentWeaponGameObject != null)
        {
            currentWeaponGameObject.gameObject.SetActive(true);
/*            hitbox.gameObject.SetActive(false);*/
        }
        else
        {
            Debug.Log("Current weapongameobject does not exist - deactivatehitbox()");
        }

        Destroy(gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                // Calculate total damage
                float totalDamage = damage;


                Debug.LogWarning("Total damage at melee: "+totalDamage);
                enemy.TakeDamage(totalDamage);
                Debug.Log($"Enemy health reduced. Current health: {enemy.health}");
            }
            else
            {
                Debug.LogWarning("Enemy script not found on the collided object.");
            }
        }
    }
}