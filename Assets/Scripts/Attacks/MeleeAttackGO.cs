using UnityEngine;

public class MeleeAttackGO : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator meleeAttackAnimator;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Melee weapon hit something");
    }

}
