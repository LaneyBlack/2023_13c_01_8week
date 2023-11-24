using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSword : Equippable
{
    [Header("Key")]
    [SerializeField] private KeyCode _keyCode;

    [Header("Attack Data")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damageValue = 1;
    [SerializeField] private int upgradedDamageValue = 2;
    [SerializeField] private float offsetFromCenter = .5f;

    [Header("Attack Layers")]
    [SerializeField] private LayerMask layerMask;
    
    private Animator animator;
    private BoxCollider2D boxCollider;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

    private float timeSinceAttack = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        //offsetFromCenter = .5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(_keyCode) && isEquipped && Time.time - timeSinceAttack >= attackCooldown)
        {
            if (InvenoryManagment.IsSwordUpgraded)
            {
                animator.SetTrigger("AttackUpgradedSword");
            }
            else
            {
                animator.SetTrigger("AttackSword");
            }
            attack();
            timeSinceAttack = Time.time;
        }
    }

    void attack()
    {
        //taken from enemy:
        var hits = Physics2D.BoxCastAll(
            boxCollider.bounds.center + transform.right * offsetFromCenter * (_sprite.flipX ? -1 : 1),
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y,
                boxCollider.bounds.size.z), // size x depends on range
            0, _rigidbody.velocity,
            0, layerMask);

        foreach (var hit in hits)
        {
            var collider = hit.collider;
            if (collider == null)
                return;

            var enemyHealth = collider.GetComponent<Health>();

            Debug.Log("We hit and its currenntHealth = " + enemyHealth.CurrentHealth);

            if (InvenoryManagment.IsSwordUpgraded)
                enemyHealth.TakeDamage(upgradedDamageValue);
            else
                enemyHealth.TakeDamage(damageValue);
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center +
            transform.right * (offsetFromCenter * (_sprite.flipX ? -1 : 1)),
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y,
                boxCollider.bounds.size.z)
            );
    }
}