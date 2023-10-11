using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private BoxCollider2D boxCollider;
    private float _cooldownTimer = 0;

    // [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer < 0 && IsPlayerInSight())
        {
            // Attack
            _cooldownTimer = attackCooldown;
        }
    }
    
    private void FixedUpdate()
    {
        // _rigidbody.velocity = new Vector2( _speed, _rigidbody.velocity.y);
    }

    private bool IsPlayerInSight()
    {
        // RaycastHit2D hitFov = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size);
        
        // Here I need to create a new collider
        return false;   
    }
    
}
