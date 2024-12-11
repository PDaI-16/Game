using UnityEngine;

public class ProjectileAttackGO : MonoBehaviour
{
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform magicProjectileFirepoint;

    [SerializeField] private GameObject rangedProjectilePrefab;
    [SerializeField] private Transform rangedProjectileFirepoint;

    private AnimationState previousState;


    private SpriteRenderer projectileSpriteRenderer;

    public void ProjectileAttack(ItemCategory weaponCategory, float attackSpeed, Camera currentCamera, AnimationState currenAnimation)
    {
        switch (weaponCategory)
        {
            case ItemCategory.Ranged:
                FireProjectile(rangedProjectilePrefab, rangedProjectileFirepoint, attackSpeed, currentCamera, -45.0f, currenAnimation);
                break;

            case ItemCategory.Magic:
                FireProjectile(magicProjectilePrefab, magicProjectileFirepoint, attackSpeed, currentCamera, 0, currenAnimation);
                break;

            default:
                Debug.LogError("Unsupported weapon category!");
                break;
        }
    }

    private void FireProjectile(GameObject projectilePrefab, Transform firepoint, float speed, Camera camera, float angleAdjust, AnimationState currentAnimation)
    {
        if (projectilePrefab != null && firepoint != null)
        {
            // Get the mouse position in world space
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure z-axis is zero for 2D

            // Calculate direction
            Vector2 direction = (mousePosition - firepoint.position).normalized;

            // Instantiate the projectile at the firepoint's position
            GameObject projectile = Instantiate(
                projectilePrefab,
                firepoint.position,
                Quaternion.identity
            );

            projectileSpriteRenderer = projectile.GetComponent<SpriteRenderer>();

            UpdateProjectileSortinglayer(currentAnimation);

            // Rotate the projectile to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleAdjust));

            // Add velocity to the projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * speed; // Use velocity to move the projectile
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or firepoint is not assigned.");
        }
    }


    void UpdateProjectileSortinglayer(AnimationState animationState)
    {
       if (previousState == animationState)
        {
            return;
        }

        previousState = animationState;

        // Adjust sorting layer and order based on animation state
        switch (animationState)
        {
            case AnimationState.player_walk_up:
            case AnimationState.player_idle_up:
            case AnimationState.player_walk_left:
            case AnimationState.player_idle_left:
                ChangeSortingLayer("PlayerWeaponBehind");
                break;

            case AnimationState.player_walk_down:
            case AnimationState.player_idle_down:
            case AnimationState.player_walk_right:
            case AnimationState.player_idle_right:
                ChangeSortingLayer("PlayerWeapon");
                break;



            default:
                ChangeSortingLayer("PlayerWeapon");
                break;
        }
    }


    public void ChangeSortingLayer(string newLayer)
    {
        if (projectileSpriteRenderer != null)
        {
            projectileSpriteRenderer.sortingLayerName = newLayer;  // Change sorting layer to the new one
        }
    }
}


