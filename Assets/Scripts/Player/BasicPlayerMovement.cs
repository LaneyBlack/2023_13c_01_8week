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
    private Transform _transform;
    private float _xInput;
    private float _currentSpeed;
    private bool _performJump;
    public bool _isGrounded;
    public bool _isFalling;
    //public Animator _animator;
    
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _runMultiplier = 1.5f;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        // _transform = GetComponentInParent<Transform>();
        _transform = transform.parent;
        //_animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && _isGrounded)
        {
            _performJump = true;
        }

        _currentSpeed = _walkSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed *= _runMultiplier;
        }

        //_animator.SetBool("Run", _xInput != 0);

        if (_xInput > 0)
        {
            _transform.localScale = new Vector3(5,5,0);
        }
        
        if (_xInput < 0)
        {
            _transform.localScale = new Vector3(-5,5,0);
        }

        _isFalling = (_rigidbody.velocity.y < -0.1f);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xInput * _currentSpeed, _rigidbody.velocity.y);

        if (_performJump)
        {
            _performJump = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

}