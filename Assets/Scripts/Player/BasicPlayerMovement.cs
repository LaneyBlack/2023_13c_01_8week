using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    private enum MovementType
    {
        Math,
        Curves,
        Old
    }

    [Header("Movement Type Pick")]
    [SerializeField] private MovementType movementType = MovementType.Math;

    [Header("Basic Ground Movement")]
    [SerializeField] private float _walkSpeed = 10;

    [Header("Ground Movement")]
    [SerializeField] private float _acceleration = 2;
    [SerializeField] private float _movementLerpMultiplier = 100;

    [Header("Ground Movement[CURVES]")]
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float accelerationTime;

    [SerializeField] private AnimationCurve deccelerationCurve;
    [SerializeField] private float deccelerationTime;

    [Header("Jump")]
    [SerializeField] [Range(1f, 10f)]  private float jumpHeight = 2;
    [SerializeField] [Range(0f, 10f)]  private float jumpRiseTime = .5f;
    [SerializeField] [Range(.1f, 10f)] private float downGravityScale = 3f;
    [SerializeField] [Range(0f, 2f)]   private float coyoteTime = 0.5f;
    [SerializeField] private List<LayerMask> jumpLayers = new List<LayerMask>();

    [Header("Hook Controls")]
    [SerializeField] private float _glideBoost = 1.5f;

    //external:
    private Rigidbody2D _rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //ground movement:
    private float _xInput;
    private float _currentSpeed;
    float accTimePassed = 0f;
    float dccTimePassed = 0f;
    bool wasMoving;

    //jumping:
    private float jumpVely = 0;
    private float regularGravity;
    private float jumpGravity;
    private float fallGravity;
    private bool _performJump = false;
    private bool _inJump = false;
    private bool wasFalling = false;
    private bool falling = false;
    private bool grounded = false;
    //coyote time:
    private float lastTimeGrounded = 0;

    //ground collision:
    private float groundRayLength = .2f;

    //hook scripts references:
    private GrapplingRope ropeScript;
    private HookGun hookScript;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ropeScript = GetComponentInChildren<GrapplingRope>(); 
        hookScript = GetComponentInChildren<HookGun>(); 
    }

    private void Start()
    {
        regularGravity = _rb.gravityScale;
        determineJumpParamters();
    }

    public float getDirection()
    {
        return _xInput;
    }

    public void jumpFullReset()
    {
        _rb.gravityScale = regularGravity;
        _inJump = false;
        wasFalling = false;
    }

    private void determineJumpParamters()
    {
        jumpVely = 2 * jumpHeight / jumpRiseTime;
        jumpGravity = -2 * jumpHeight / (jumpRiseTime * jumpRiseTime);
        fallGravity = jumpGravity * downGravityScale;
        //fallGravity = regularGravity * downGravityScale * Physics.gravity.y;
    }

    private void Update()
    {
        coyoteTimerSetUp();

        _xInput = Input.GetAxis("Horizontal");

        grounded = isGrounded();

        jumpSetUp();

        _currentSpeed = _walkSpeed;

        if (ropeScript.isGrappling)
            _currentSpeed *= _glideBoost;

        falling = (_rb.velocity.y < -0.15f);

        if (movementType == MovementType.Math)
            handleGroundMovementMath();
        else if (movementType == MovementType.Curves)
            handleMovementCurves();
        else if (movementType == MovementType.Old)
            _rb.velocity = new Vector3(_currentSpeed * _xInput, _rb.velocity.y, 0);

        handleJump();

        Flip(_xInput);
        handleAnimator();
    }

    private void FixedUpdate()
    {
        //if (movementType == MovementType.Math)
        //    handleGroundMovementMath();
        //else if (movementType == MovementType.Curves)
        //    handleMovementCurves();
        //else if (movementType == MovementType.Old)
        //    _rb.velocity = new Vector3(_currentSpeed * _xInput, _rb.velocity.y, 0);
    }

    void handleAnimator()
    {
        animator.SetBool("Run", _rb.velocity.x != 0);

        animator.SetBool("Falling", falling);
        animator.SetBool("Grounded", isGrounded());
    }

    //requires further working
    private void handleGroundMovementMath()
    {
        //var acceleration = isGrounded() ? _acceleration : _acceleration * 0.5f;
        var acceleration = _acceleration; //ignore if in air

        if (Input.GetKey(KeyCode.A))
        {
            if (_rb.velocity.x > 0) _xInput = 0; 
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

        _rb.velocity = Vector3.MoveTowards(_rb.velocity, wishVelocity, _movementLerpMultiplier * Time.deltaTime);
        //_rb.velocity = wishVelocity;
    }

    private void handleMovementCurves()
    {
        Debug.Log(dccTimePassed);
        //float xvel = 0;
        if(_xInput != 0)
        {
            dccTimePassed = 0;
            accTimePassed = Mathf.Clamp(accTimePassed + Time.deltaTime, 0, accelerationTime);
            _xInput *= accelerationCurve.Evaluate(accTimePassed / accelerationTime);

            //xvel = accelerationCurve.Evaluate(accTimePassed / accelerationTime) * _xInput;
            //wasMoving = true;
            //_rb.velocity = new Vector2(xvel * _xInput, _rb.velocity.y);
        }
        else 
        {
            accTimePassed = 0;
            dccTimePassed = Mathf.Clamp(dccTimePassed + Time.deltaTime, 0, deccelerationTime);
            _xInput *= deccelerationCurve.Evaluate(dccTimePassed / deccelerationTime);

            //xvel = deccelerationCurve.Evaluate(dccTimePassed / deccelerationTime) * _xInput;
            //_rb.velocity = new Vector2(xvel, _rb.velocity.y);
        }

        _rb.velocity = new Vector2(_xInput * _currentSpeed, _rb.velocity.y);



        //if(xvel == 0)
        //    wasMoving = false;

        //if (Input.GetKey(KeyCode.A))
        //{
        //    dccTimePassed = 0;
        //    accTimePassed =  Mathf.Clamp(accTimePassed + Time.deltaTime, 0, accelerationTime);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    dccTimePassed = 0;
        //    accTimePassed = Mathf.Clamp(accTimePassed + Time.deltaTime, 0, accelerationTime);
        //}
    }

    private void coyoteTimerSetUp()
    {
        if (grounded && !isGrounded() && !_inJump && _rb.velocity.y < 0.1f)        //was grounded on the previous frame and now isnt
            lastTimeGrounded = Time.time;
    }

    private void jumpSetUp()
    {
        if (Input.GetButtonDown("Jump") && (grounded || ropeScript.isGrappling || Time.time - lastTimeGrounded <= coyoteTime))
        {
            _performJump = true;
        }
    }

    private void handleJump()
    {
        float g = Physics.gravity.y;
        if (_performJump)
        {
            _performJump = false;
            hookScript.disableGrappling();
            _inJump = true;

            _rb.gravityScale = jumpGravity / g;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpVely);
        }
        else if (falling && _inJump)
        {
            wasFalling = true;
            _rb.gravityScale = fallGravity / g;
        }
        else if (_inJump && wasFalling && isGrounded())
        {
            jumpFullReset();
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