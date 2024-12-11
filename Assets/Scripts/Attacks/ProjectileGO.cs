using UnityEngine;

public class ProjectileGO : MonoBehaviour
{

    private float totalDamage;

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
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }


}
