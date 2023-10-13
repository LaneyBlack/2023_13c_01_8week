using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float movementSpeed;

    // Hit Fov ---
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float hitFovRange;
    [SerializeField] private float hitFovDistance;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private float attackCooldown;
    private float _cooldownTimer;

    private Rigidbody2D _rigidbody;
    
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer < 0 && IsPlayerInSight())
        {
            // Attack
            _cooldownTimer = attackCooldown;
            _animator.SetTrigger("meleeAttack");
        }
    }
    
    private void FixedUpdate()
    {
        // _rigidbody.velocity = new Vector2( _speed, _rigidbody.velocity.y);
    }

    private bool IsPlayerInSight()
    {
        // Origin, Size, Angle, direction
        var hitFov = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * (hitFovDistance * transform.localScale.x), //
            new Vector3(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), // size x depends on range
            0,Vector2.left,
            0,layerMask);
        return hitFov.collider!=null;
    }

    // For Debugging purposes
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Draw Hit Fov
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * (hitFovDistance * transform.localScale.x),
            new Vector2(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y));
    }
    
}
