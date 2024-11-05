using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    [SerializeField] Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
