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
    private float _xInput;
    private bool _performJump;
    public bool _isGrounded;
    
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
    }


    // Update is called once per frame
    private void Update()
    {
        _xInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
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

}