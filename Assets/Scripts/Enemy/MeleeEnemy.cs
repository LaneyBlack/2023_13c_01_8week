using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Movement and Player interaction")] [SerializeField]
    private float movementSpeed;

    [SerializeField] public GameObject player;
    [SerializeField] private float maxFollowDistance;
    [SerializeField] private float minFollowDistance;
    private float _distance;

    [Header("HitFov")] [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float hitFovRange;
    [SerializeField] private float hitFovDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float hitForce = 15;

    [Header("Attacking")] [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    private float _cooldownTimer;
    private Health _playerHealth;

    private Health _health;
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
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Return if dead
        if (_health.IsDead())
            return;

        _distance = Vector2.Distance(transform.position, player.transform.position);

        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer < 0 && HasPlayerInSight() && !_health.IsDead())
        {
            // Attack
            _cooldownTimer = attackCooldown;
            _animator.SetTrigger(MeleeAttack); // attack animation
        }

        // Move towards player if distance is not too small and not to big
        var moving = (_distance < maxFollowDistance && _distance > minFollowDistance) && !_animator.GetBool(IsFalling);
        _animator.SetBool(IsMoving, moving); // moving animation value
        if (moving)
        {
            _sprite.flipX = (transform.position.x - player.transform.position.x) < 0; // face forward
        }

        // Falling animation value
        _animator.SetBool(IsFalling, _rigidbody.velocity.y < -0.1f);
    }

    private void FixedUpdate()
    {
        if (_animator.GetBool(IsMoving) && !_health.IsDead())
        {
            var direction = player.transform.position - transform.position; // Get direction to move in
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
            _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            // _rigidbody.AddForce(new Vector2(movementSpeed * Math.Sign(direction.x), 0), ForceMode2D.Force); // didnt like, but we can do some tests
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private bool HasPlayerInSight()
    {
        // Box Cast creation
        var hitFov = Physics2D.BoxCast(
            boxCollider.bounds.center +
            transform.right * (hitFovDistance * transform.localScale.x * (_sprite.flipX ? -1 : 1)),
            new Vector3(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y,
                boxCollider.bounds.size.z), // size x depends on range
            0, _rigidbody.velocity,
            0, layerMask);
        // If hitfov sees player collider, get player Health
        if (hitFov.collider != null) {
            _playerHealth = hitFov.transform.GetComponent<Health>();
            return true;
        }
        // If get collider is null there is no player in it
        return false;
    }

    private void DamagePlayer()
    {
        if (HasPlayerInSight())
        {
            _playerHealth.TakeDamage(damage); // Player take damage
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(hitForce * (_sprite.flipX?2:-2), hitForce), ForceMode2D.Impulse); // Kicks player
        }
    }

    private void TakeDamage()
    {
        _animator.SetTrigger(TakeHit);
    }

    // // For Debugging purposes only ---
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     // Draw Hit Fov
    //     Gizmos.DrawWireCube(
    //         boxCollider.bounds.center +
    //         transform.right * (hitFovDistance * transform.localScale.x * (_sprite.flipX ? -1 : 1)),
    //         new Vector3(boxCollider.bounds.size.x * hitFovRange, boxCollider.bounds.size.y,
    //             boxCollider.bounds.size.z));
    // }
}