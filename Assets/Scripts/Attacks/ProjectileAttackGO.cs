using UnityEngine;

public class ProjectileAttackGO : MonoBehaviour
{
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform magicProjectileFirepoint;

    [SerializeField] private GameObject rangedProjectilePrefab;
    [SerializeField] private Transform rangedProjectileFirepoint;

    public void ProjectileAttack(ItemCategory weaponCategory, float attackSpeed, Camera currentCamera)
    {
        switch (weaponCategory)
        {
            case ItemCategory.Ranged:
                FireProjectile(rangedProjectilePrefab, rangedProjectileFirepoint, attackSpeed, currentCamera);
                break;

            case ItemCategory.Magic:
                FireProjectile(magicProjectilePrefab, magicProjectileFirepoint, attackSpeed, currentCamera);
                break;

            default:
                Debug.LogError("Unsupported weapon category!");
                break;
        }
    }

    private void FireProjectile(GameObject projectilePrefab, Transform firepoint, float speed, Camera camera)
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

            // Rotate the projectile to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

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
}


