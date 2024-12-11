using UnityEngine;

public class ProjectileAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform magicProjectileFirepoint;

    [SerializeField] private GameObject rangedProjectilePrefab;
    [SerializeField] private GameObject rangedProjectileFirepoint;


    public void ProjectileAttack(ItemCategory weaponCategory, float attackSpeed, Camera currentCamera)
    {
        switch (weaponCategory)
        {
            case ItemCategory.Ranged:
                break;

            case ItemCategory.Magic:
                if (magicProjectilePrefab != null && magicProjectileFirepoint != null)
                {
                    // Get the mouse position in world space
                    Vector3 mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0; // Ensure the z-axis is zero for 2D space

                    // Calculate the direction from the firepoint to the mouse position
                    Vector2 direction = (mousePosition - magicProjectileFirepoint.position).normalized;

                    // Instantiate the magic projectile at the firepoint's position
                    GameObject magicProjectile = Instantiate(
                        magicProjectilePrefab,
                        magicProjectileFirepoint.position,
                        Quaternion.identity // Set rotation to default; you can customize if needed
                    );

                    // Rotate the projectile to face the direction of the mouse
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    magicProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    // Add velocity to the projectile
                    Rigidbody2D rb = magicProjectile.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = direction * attackSpeed; // Set the projectile's velocity
                    }
                }
                else
                {
                    Debug.LogError("Magic projectile prefab or firepoint is not assigned.");
                }
                break;
        }

    }

}


