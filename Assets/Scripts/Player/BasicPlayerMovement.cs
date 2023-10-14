using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float _xInput;
    private bool _performJump;
    private bool _isGrounded;
    
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump"))
        {
            _performJump = true;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_xInput * _speed, _rigidbody.velocity.y);

        if (_performJump && _isGrounded)
        {
            _performJump = false;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        _isGrounded = false;
    }
}