using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class BasicPlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private BoxCollider2D boxCollider;
    private float _xInput;
    private float _currentSpeed;
    private float _jumpForce;
    private bool _performJump;
    public bool _isGrounded { get; private set; }
    public bool _isFalling { get; private set; }
    //public Animator _animator;

    private float groundRayLength = .1f;

    //DEBUG:
    int scount = 0;
    int jcount = 0;

    [SerializeField] private GrapplingRope grapplingRope;
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _basicJumpForce = 10;
    [SerializeField] private float _runMultiplier = 1.5f;
    [SerializeField] private float _grappleMultiplier = 1.5f;
    [SerializeField] private float _grappleJumpBoost = 1.3f;
    [SerializeField] private LayerMask groundLayer;
    

    //[SerializeField] private bool groundedOnAnything = true;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();
    }

    private void Update()
    {
        _isGrounded = isGrounded();

        _xInput = Input.GetAxis("Horizontal");
        _jumpForce = _basicJumpForce;

        if (Input.GetButtonUp("Jump") && (_isGrounded || grapplingRope.isGrappling))
        {
            _performJump = true;
            if (grapplingRope.isGrappling)
                _jumpForce *= _grappleJumpBoost;

            //DEBUG:
            scount++;
        }

        _currentSpeed = _walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
            _currentSpeed *= _runMultiplier;

        if (grapplingRope.isGrappling)
            _currentSpeed *= _grappleMultiplier;

        _isFalling = (_rigidbody.velocity.y < 0);

        //DEBUG:
       // Debug.Log("space: " + scount + "\t jump: " + jcount);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xInput * _currentSpeed, _rigidbody.velocity.y);

        if (_performJump)
        {
            _performJump = false;
            grapplingRope.enabled = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            
            //DEBUG:
            jcount++;
        }
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundRayLength, groundLayer);
    }
}