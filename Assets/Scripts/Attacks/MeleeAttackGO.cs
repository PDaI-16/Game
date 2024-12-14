using UnityEngine;


public class MeleeAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Animator meleeAttackAnimator;
    private GameObject currentWeaponGameObject;
    [SerializeField] private Camera currentCamera;

    [SerializeField] private GameObject enemyImpactPrefab;
    [SerializeField] private GameObject treeImpactPrefab;
    

    [SerializeField] private AnimationState previousState;
    private Transform parentTransform;


    [SerializeField] private GameObject damageIndicatorPrefab;

    private Vector3 positionAdjust = new Vector3(0, 1.0f, 0);

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
        if (collision.CompareTag("tree"))
        {
            GameObject tree = collision.gameObject;
            if (tree != null)
            {
                Vector3 direction = (transform.position - tree.transform.position).normalized; // Direction towards the transform
                Vector3 adjustedPosition = tree.transform.position + direction * 1f; // Move prefab closer by offsetDistance

                if (treeImpactPrefab != null)
                {
                    GameObject impactClone =
                    Instantiate(
                        treeImpactPrefab,
                        adjustedPosition,
                        Quaternion.identity
                    );
                    if (impactClone != null)
                    {
                        impactClone.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                    }

                }
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                Vector3 direction = (transform.position - enemy.transform.position).normalized; // Direction towards the transform
                Vector3 adjustedPosition = enemy.transform.position + direction * 1f; // Move prefab closer by offsetDistance

                // Calculate total damage
                float totalDamage = damage;

                enemy.TakeDamage(totalDamage);

                if (damageIndicatorPrefab != null)
                {
                    GameObject damageIndicatorClone =
                        Instantiate(
                            damageIndicatorPrefab,
                            collision.GetComponent<Transform>().position + positionAdjust,
                            Quaternion.identity
                        ); ;
                    damageIndicatorClone.GetComponent<DamageIndicatorGO>().SetDamageText(totalDamage);
                }

                if (enemyImpactPrefab != null)
                {


                    GameObject impactClone =
                    Instantiate(
                        enemyImpactPrefab,
                        adjustedPosition,
                        Quaternion.identity
                    );

                    if (impactClone != null)
                    {
                        impactClone.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                    }
                }


            }
            else
            {
                Debug.LogWarning("Enemy script not found on the collided object.");
            }
        }
    }
}