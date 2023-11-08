using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxFollowDistance;
    [SerializeField] private float minFollowDistance;
   
    
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;
    
    private Rigidbody2D _rigidbody;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetBool(IsMoving, IsInSight());
        _sprite.flipX = ( transform.parent.position.x - player.transform.position.x) < 0;
    }

    private void FixedUpdate()
    {
        if (_animator.GetBool(IsMoving))
        {
            var direction = player.transform.position - transform.parent.position; 
            _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            Debug.Log(_rigidbody.velocity);
        }
    }


    private bool IsInSight()
    {
        var distance = Vector2.Distance( transform.parent.position, player.transform.position);
        var isInSight = distance < maxFollowDistance && distance > minFollowDistance;
        return isInSight;
    }
}