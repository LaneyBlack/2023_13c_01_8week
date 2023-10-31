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
    //private Transform _transform;
    private float _xInput;
    private float _currentSpeed;
    private float _jumpForce;
    private bool _performJump;
    public bool _isGrounded; /*{ get; private set; }*/
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

    //[SerializeField] private bool groundedOnAnything = true;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        boxCollider = GetComponentInParent<BoxCollider2D>();
        //groundRayLength = _boxCollider.transform.localScale.y / 2 * _boxCollider.size.y;

        //DEBUG:
        //Debug.Log(groundRayLength);
    }


    // Update is called once per frame
    private void Update()
    {
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
        Debug.Log("space: " + scount + "\t jump: " + jcount);
    }

    private void FixedUpdate()
    {
        var origin = _rigidbody.position - (boxCollider.transform.localScale * new Vector2(0, 0.01f + boxCollider.size.y / 2));

        //DEBUG:
        Debug.DrawRay(origin, Vector2.down * groundRayLength);

        var hit = Physics2D.Raycast(origin, Vector2.down, groundRayLength);

        //if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //    _isGrounded = true;
        //else
        //    _isGrounded = false;

        _isGrounded = hit.collider != null;

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
}