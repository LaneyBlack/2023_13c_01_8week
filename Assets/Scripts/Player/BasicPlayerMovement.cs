using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    //DEBUG:
    int scount = 0;
    int jcount = 0;  

    private enum MovementType
    {
        Physics,
        Math,
        Old
    }

    private enum JumpType
    {
        Old,
        New
    }


    [Header("Movement Type Pick")]
    [SerializeField] private MovementType movementType = MovementType.Math;


    [Header("Basic Ground Movement")]
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _runMultiplier = 1.5f;

    [Header("Math Ground Movement")]
    [SerializeField] private float _accelerationMath = 2;
    [SerializeField] private float _movementLerpMultiplier = 100;

    [Header("Physics Ground Movement")]
    [SerializeField] private float _accelerationPhys = 2;
    [SerializeField] private float _deccelarationPhys = 2;
    [SerializeField] private float velPower = 1;


    [Header("Jump Type Pick")]
    [SerializeField] private JumpType jumpType = JumpType.New;

    [Header("Jump")]
    [SerializeField] private float _basicJumpForce = 10;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float jumpRiseTime = .5f;
    [SerializeField] private float downGravityScale = 3f;
    
    [SerializeField] private List<LayerMask> jumpLayers = new List<LayerMask>();


    [Header("Grappling Rope")]
    [SerializeField] private GrapplingRope ropeScript;
    [SerializeField] private float _glideBoost = 1.5f;
    [SerializeField] private float _jumpBoost = 1.3f;


    private Rigidbody2D _rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float _xInput;
    private float _currentSpeed;

    private float _jumpForce;
    private bool _performJump = false;
    private bool _inJump = false;
    private bool wasFalling = false;
    
    private bool falling = false;

    private float groundRayLength = .2f;

    private float jumpVely = 0;
    private float regularGravity;
    private float jumpGravity;
    private float fallGravity;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        regularGravity = _rb.gravityScale;
        _rb.velocity = Vector3.zero;
        determineJumpParamters();
    }

    void determineJumpParamters()
    {
        jumpVely = 2 * jumpHeight / jumpRiseTime;
        jumpGravity = -2 * jumpHeight / (jumpRiseTime * jumpRiseTime);
        fallGravity = jumpGravity * downGravityScale;
        //fallGravity = regularGravity * downGravityScale * Physics.gravity.y;
    }

    private void Update()
    {
        //determineJumpParamters();

        _xInput = Input.GetAxis("Horizontal");
        Flip(_xInput);

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

        if(movementType == MovementType.Math)
            handleGroundMovementMath();

        if (ropeScript.isGrappling)
            _currentSpeed *= _glideBoost;

        falling = (_rb.velocity.y < -0.15f);

        if (jumpType == JumpType.New)
            handleJump();

        handleAnimator();

        //DEBUG:
        //Debug.Log("space: " + scount + "\t jump: " + jcount);
        //Debug.Log(_rb.gravityScale);
    }

    void handleAnimator()
    {
        animator.SetBool("Run", _rb.velocity.x != 0);

        //set animator transitions:
        animator.SetBool("Falling", falling);
        animator.SetBool("Grounded", isGrounded());
    }

    //requires further working
    void handleGroundMovementMath()
    {
        // Slowly release control after wall jump
        //_currentMovementLerpSpeed = Mathf.MoveTowards(_currentMovementLerpSpeed, 100, _wallJumpMovementLerp * Time.deltaTime);

        // This can be done using just X & Y input as they lerp to max values, but this gives greater control over velocity acceleration
        var acceleration = isGrounded() ? _accelerationMath : _accelerationMath * 0.5f;

        if (Input.GetKey(KeyCode.A))
        {
            if (_rb.velocity.x > 0) _xInput = 0; // Immediate stop and turn. Just feels better
            _xInput = Mathf.MoveTowards(_xInput, -1, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_rb.velocity.x < 0) _xInput = 0;
            _xInput = Mathf.MoveTowards(_xInput, 1, acceleration * Time.deltaTime);
        }
        else
        {
            _xInput = Mathf.MoveTowards(_xInput, 0, acceleration * Time.deltaTime);
        }

        var wishVelocity = new Vector3(_xInput * _currentSpeed, _rb.velocity.y);
        // _currentMovementLerpSpeed should be set to something crazy high to be effectively instant. But slowed down after a wall jump and slowly released
        _rb.velocity = Vector3.MoveTowards(_rb.velocity, wishVelocity, _movementLerpMultiplier * Time.deltaTime);

    }

    void handleMovementGroundPhysics()
    {
        float targetSpeed = _xInput * _currentSpeed;
        float speeddiff = targetSpeed - _rb.velocity.x;
        float acc = (Mathf.Abs(targetSpeed) > 0.01f) ? _accelerationPhys : _deccelarationPhys;

        float movement = Mathf.Pow(Mathf.Abs(speeddiff) * acc, velPower) * Mathf.Sign(speeddiff);
        _rb.AddForce(movement * Vector2.right);
    }

    void handleJump()
    {
        float g = Physics.gravity.y;
        if (_performJump)
        {
            _performJump = false;
            ropeScript.enabled = false;
            _inJump = true;

            _rb.gravityScale = jumpGravity / g;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpVely);

            //DEBUG:
            jcount++;
        }
        else if (falling && _inJump)
        {
            wasFalling = true;
            _rb.gravityScale = fallGravity / g;
        }
        else if (_inJump && isGrounded() && wasFalling)
        {
            _rb.gravityScale = regularGravity;
            _inJump = false;
            wasFalling = false;

        }
    }

    private void FixedUpdate()
    {
        if(movementType == MovementType.Old)
            _rb.velocity = new Vector2(_xInput * _currentSpeed, _rb.velocity.y);
        if (movementType == MovementType.Physics)
            handleMovementGroundPhysics();

        if (jumpType == JumpType.Old)
        {
            if (_performJump)
            {
                _performJump = false;
                ropeScript.enabled = false;
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);

                //DEBUG:
                jcount++;
            }
        }
    }

    private bool isGrounded()
    {
        var collided = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundRayLength); //<- change to raycast??
        foreach (var col in collided)
        {
            if (col.collider == null) continue;
            if (jumpLayers.Contains(LayerMask.GetMask(LayerMask.LayerToName(col.collider.gameObject.layer))))
            {
                //Debug.Log(LayerMask.LayerToName(col.collider.gameObject.layer));
                return true;
            }
        }

        return false;
    }

    private void Flip(float direction)
    {
        if (direction > 0)
            spriteRenderer.flipX = false;
        else if (direction < 0)
            spriteRenderer.flipX = true;
    }
}