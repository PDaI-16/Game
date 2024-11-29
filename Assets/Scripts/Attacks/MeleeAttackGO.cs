using UnityEngine;

public class MeleeAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator meleeAttackAnimator;
    private Vector3 shouldBePosition;

    void Start()
    {
        meleeAttackAnimator = GetComponent<Animator>();
        Attack();
    }


    // This activated from event at the end of the animation
    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }

    void Attack()
    {
        meleeAttackAnimator.Play("Melee attack");
    }
/*    void Update()
    {
        transform.position = shouldBePosition;
    }

    public void UpdateAttackPosition(Vector3 position)
    {   
        shouldBePosition = position;
    
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Melee weapon hit something");
    }

}
