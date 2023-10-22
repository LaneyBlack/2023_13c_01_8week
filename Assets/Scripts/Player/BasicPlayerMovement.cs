using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    //private Transform _transform;
    private float _xInput;
    private float _currentSpeed;
    private float _jumpForce;
    private bool _performJump;
    public bool _isGrounded;
    public bool _isFalling;
    //public Animator _animator;

    [SerializeField] private GrapplingRope grapplingRope;
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _basicJumpForce = 10;
    [SerializeField] private float _runMultiplier = 1.5f;
    [SerializeField] private float _grappleMultiplier = 1.5f;
    [SerializeField] private float _grappleJumpBoost = 1.3f;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        // _transform = GetComponentInParent<Transform>();
        //_transform = transform.parent;
    }


    // Update is called once per frame
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");
        _jumpForce = _basicJumpForce;
        //Debug.Log(_jumpForce);

        if (Input.GetButton("Jump") && (_isGrounded || grapplingRope.isGrappling))
        {
            _performJump = true;
            if (grapplingRope.isGrappling)
                _jumpForce *= _grappleJumpBoost;
        }

        _currentSpeed = _walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
            _currentSpeed *= _runMultiplier;

        if (grapplingRope.isGrappling)
            _currentSpeed *= _grappleMultiplier;

        //if (_xInput > 0)
        //{
        //    _transform.localScale = new Vector3(5,5,0);
        //}

        //if (_xInput < 0)
        //{
        //    _transform.localScale = new Vector3(-5,5,0);
        //}

        _isFalling = (_rigidbody.velocity.y < 0);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xInput * _currentSpeed, _rigidbody.velocity.y);

        if (_performJump)
        {
            _performJump = false;
            grapplingRope.enabled = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

}