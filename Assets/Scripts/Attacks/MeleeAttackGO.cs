using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MeleeAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Animator meleeAttackAnimator;
    [SerializeField] public BoxCollider2D hitbox;
    private GameObject currentWeaponGameObject;
    [SerializeField] private Camera currentCamera;

    [SerializeField] private AnimationState previousState;

    private Weapon usedWeaponData;

    void Start()
    {

        if (hitbox == null)
        {
            Debug.LogError("Hitbox collider2D not found");
            return;
        }
    }


    public void Attack(Weapon usedWeapon, AnimationState playerAnimationState, GameObject currentWeaponObject, Camera camera)
    {
        usedWeaponData = usedWeapon;
        currentCamera = camera;
        currentWeaponGameObject = currentWeaponObject;
        currentWeaponObject.SetActive(false);


        FlipMeleeAttack(playerAnimationState);
        ObjectRotateAccordingToMouse.RotateObjectForMeleeAttack(gameObject.transform, currentCamera);


        meleeAttackAnimator.Play("Melee attack");
        Debug.Log("Melee attack with damage of: " + usedWeapon.Damage);

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
            hitbox.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Current weapongameobject does not exist - deactivatehitbox()");
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Melee weapon hit something");
        if (collision.CompareTag("Enemy"))
        {
            // Get the enemy's script that contains the health variable
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                // Reduce the enemy's health
                enemy.TakeDamage(usedWeaponData.Damage);
                Debug.Log("Enemy health reduced. Current health: " + enemy.health);
            }
            else
            {
                Debug.LogWarning("Enemy script not found on the collided object.");
            }
        }


    }

}