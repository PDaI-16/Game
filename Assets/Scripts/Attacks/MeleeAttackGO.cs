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
    [SerializeField] private float critChance = 0.1f; // Default crit chance (10%)
    private float critMultiplier = 2f; // 2x damage for crits


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
    public void SetCritChance(float bonusCritChance)
    {
        critChance += bonusCritChance;
        // Ensure the crit chance doesn't exceed 100% (1.0f)
        critChance = Mathf.Min(critChance, 1f);
        Debug.Log($"Critical chance updated to: {critChance * 100}%");
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
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                // Calculate total damage
                float totalDamage = usedWeaponData.Damage + skillDamageBonus;

                // Determine if this attack is a critical hit
                if (IsCriticalHit())
                {
                    totalDamage *= critMultiplier;  // Apply critical hit damage multiplier
                    Debug.Log("Critical hit! Damage doubled.");
                }

                // Apply the damage to the enemy
                enemy.TakeDamage(totalDamage);
                Debug.Log($"Enemy health reduced. Current health: {enemy.health}");
            }
            else
            {
                Debug.LogWarning("Enemy script not found on the collided object.");
            }
        }
    }
    // Method to check if the hit is a critical hit (10% chance)
    private bool IsCriticalHit()
    {
        return Random.value < critChance; // Random.value gives a float between 0 and 1
    }
}