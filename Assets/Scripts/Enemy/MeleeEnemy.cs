using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Movement and Player interaction")] [SerializeField]
    private float movementSpeed;

    [SerializeField] private GameObject player;
    [SerializeField] private float maxFollowDistance;
    [SerializeField] private float minFollowDistance;
    private float _distance;

    [Header("HitFov")] [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float hitFovRange;
    [SerializeField] private float hitFovDistance;
    [SerializeField] private LayerMask layerMask;

    [Header("Attacking")] [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    private float _cooldownTimer;
    private Health _playerHealth;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;
    private Animator _animator;

    private static readonly int MeleeAttack = Animator.StringToHash("meleeAttack");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int TakeHit = Animator.StringToHash("takeHit");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        _distance = Vector2.Distance(transform.position, player.transform.position);

        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer < 0 && HasPlayerInSight())
        {
            // Attack
            _cooldownTimer = attackCooldown;
            _animator.SetTrigger(MeleeAttack); // attack animation
        }

        // Move towards player if distance is not too small and not to big
        var moving = (_distance < maxFollowDistance && _distance > minFollowDistance) && !_animator.GetBool(IsFalling);
        _animator.SetBool(IsMoving, moving); // moving animation value
        if (moving)
            _sprite.flipX = (transform.position.x - player.transform.position.x) < 0; // face forward

        // Falling animation value
        var a = _rigidbody.velocity.y < -0.1f;
        _animator.SetBool(IsFalling, a);
    }

    private void FixedUpdate()
    {
        if (_animator.GetBool(IsMoving))
        {
            var direction = player.transform.position - transform.position; // Get direction to move in
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
            _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private bool HasPlayerInSight()
    {
        var hitFov = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * (hitFovDistance * transform.localScale.x), //
            new Vector3(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y,
                boxCollider.bounds.size.z), // size x depends on range
            0, Vector2.left,
            0, layerMask);
        if (hitFov.collider != null) // prep for the merge with player (DO NOT DELETE)
            _playerHealth = hitFov.transform.GetComponent<Health>();

        // If get collider is not null there is player in it
        return hitFov.collider != null;
    }

    // For Debugging purposes only
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     // Draw Hit Fov
    //     Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * (hitFovDistance * transform.localScale.x),
    //         new Vector2(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y));
    // }

    private void DamagePlayer()
    {
        if (HasPlayerInSight())
        {
            _playerHealth.TakeDamage(damage);
        }
    }

    private void TakeDamage()
    {
        _animator.SetTrigger(TakeHit);
    }
}