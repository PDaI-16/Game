using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] int movementSpeed;
    
    
    private Vector2 _movementInput;
    private Animator _playerAnimator;
    private Camera _mainCamera;

    private Vector3 _mousePosition;
    private Vector3 _screenPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
        movementSpeed = 2;
        _playerAnimator = GetComponent<Animator>();
        
    } // Update is called once per frame
    void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        _movementInput.Normalize(); // Normalizing the movement input so that diagonal movement is not faster than others.

        playerRigidbody.linearVelocity = _movementInput * movementSpeed;

        _mousePosition = Input.mousePosition;
        _screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);

        FlipPlayer();
        AnimationHandler();
       }
   void AnimationHandler()
    {

        if(_movementInput.x == 0 && _movementInput.y == 0)
        {
            _playerAnimator.SetInteger("walkingState", 0); // Idle
        }
        else if(_movementInput.x != 0)
        {
            _playerAnimator.SetInteger("walkingState", 1); // Walk right or left
        }
        else if(_movementInput.y > 0 && _movementInput.x == 0)
        {
            _playerAnimator.SetInteger("walkingState", 2); // Walk up
        }
        else if(_movementInput.y < 0 && _movementInput.x == 0)
        {
            _playerAnimator.SetInteger("walkingState", 3); // Walk down
        }

    }

    void FlipPlayer()
    {

        if (_mousePosition.x < _screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }            
    }
}
