using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSword : Equippable
{
    [Header("Key")]
    [SerializeField] private KeyCode _keyCode;

    [Header("Attack Data")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damageValue = 1;
    [SerializeField] private float attackCooldown;
    
    [Header("Attack Layers")]
    [SerializeField] private LayerMask enemyLayers;
    
    private Animator animator;
    private float timeSinceAttack = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_keyCode) && isEquipped && Time.time - timeSinceAttack >= attackCooldown)
        {
            animator.SetTrigger("AttackSword");
            attack();
            timeSinceAttack = Time.time;
        }
    }

    void attack()
    {
       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
       foreach (Collider2D enemy in hitEnemies)
       {
           Debug.Log("We hit and its currenntHealth = " + enemy.GetComponent<Health>().CurrentHealth);
           enemy.GetComponent<Health>().TakeDamage(damageValue);
       }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}