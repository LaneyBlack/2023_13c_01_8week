using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    //DEBUG:
    int scount = 0;
    int jcount = 0;

    [Header("Visuals References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;


    [Header("Ground Movement")]
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _runMultiplier = 1.5f;


    [Header("Jump")]
    [SerializeField] private float _basicJumpForce = 10;
    [SerializeField] private LayerMask jumpLayer;


    [Header("Grappling Rope")]
    [SerializeField] private GrapplingRope ropeScript;
    [SerializeField] private float _glideBoost = 1.5f;
    [SerializeField] private float _jumpBoost = 1.3f;


    private Rigidbody2D _rigidbody;
    private BoxCollider2D boxCollider;
    private float _xInput;
    private float _currentSpeed;
    private float _jumpForce;
    private bool _performJump;
    private float groundRayLength = .1f;


    //[SerializeField] private bool groundedOnAnything = true;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();
    }

    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");
        Flip(_rigidbody.velocity.x);
        animator.SetBool("Run", _rigidbody.velocity.x != 0);

        var grounded = isGrounded();
        _jumpForce = _basicJumpForce;

        if (Input.GetButtonDown("Jump") && (grounded || ropeScript.isGrappling))
        {
            _performJump = true;
            if (ropeScript.isGrappling)
                _jumpForce *= _jumpBoost;

            //DEBUG:
            scount++;
        }

        _currentSpeed = _walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
            _currentSpeed *= _runMultiplier;

        if (ropeScript.isGrappling)
            _currentSpeed *= _glideBoost;

        //set animator transitions:
        animator.SetBool("Falling", (_rigidbody.velocity.y < 0));
        animator.SetBool("Grounded", grounded);

        //DEBUG:
        //Debug.Log("space: " + scount + "\t jump: " + jcount);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xInput * _currentSpeed, _rigidbody.velocity.y);

        if (_performJump)
        {
            _performJump = false;
            ropeScript.enabled = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            
            //DEBUG:
            jcount++;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundRayLength, jumpLayer);
    }

    private void Flip(float direction)
    {
        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}