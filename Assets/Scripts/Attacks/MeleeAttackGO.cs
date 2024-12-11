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
    private float skillDamageBonus = 0f;


    private Weapon usedWeaponData;

    void Start()
    {

        if (hitbox == null)
        {
            Debug.LogError("Hitbox collider2D not found");
            return;
        }
    }

    public void SetSkillDamageBonus(float bonus)
    {
        skillDamageBonus = bonus;
        Debug.Log($"Skill damage bonus updated to: {skillDamageBonus}");
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
        Debug.Log("Melee attack with total damage of: " + (usedWeapon.Damage + skillDamageBonus));
    }

    private void FlipMeleeAttack(AnimationState playerAnimationState)
    {
        transform.localScale = new Vector3(1f, 1f, 0);

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
        currentWeaponGameObject.gameObject.SetActive(true);
        hitbox.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Melee weapon hit something");
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                // Reduce the enemy's health with bonus damage included
                enemy.TakeDamage(usedWeaponData.Damage + skillDamageBonus);
                Debug.Log($"Enemy health reduced. Current health: {enemy.health}");
            }
            else
            {
                Debug.LogWarning("Enemy script not found on the collided object.");
            }
        }
    }
}