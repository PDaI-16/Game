using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileGO : MonoBehaviour
{

    private float totalDamage;

    [SerializeField] private GameObject damageIndicatorPrefab;
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private GameObject enemyImpactPrefab;

    private Vector3 positionAdjust = new Vector3(0, 0.4f,0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
     
       
    }

    public void SetDamage(float damage)
    {
        totalDamage = damage;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tree"))
        {
            Destroy(gameObject);

            if (impactPrefab != null)
            {
                GameObject impactClone =
                Instantiate(
                    impactPrefab,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();

            if (enemy != null)
            {
                enemy.TakeDamage(totalDamage);
                if (damageIndicatorPrefab != null)
                {
                    GameObject damageIndicatorClone = 
                        Instantiate(
                            damageIndicatorPrefab,
                            transform.position+positionAdjust,
                            Quaternion.identity
                        );
                    damageIndicatorClone.GetComponent<DamageIndicatorGO>().SetDamageText(totalDamage);

                    if (enemyImpactPrefab != null)
                    {
                        GameObject impactClone =
                        Instantiate(
                            enemyImpactPrefab,
                            transform.position,
                            Quaternion.identity
                        );
                    }
                }

            }




            Destroy(gameObject);
        }
    }


}
