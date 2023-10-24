using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSword : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    [SerializeField] private KeyCode _keyCode;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] public int damageValue = 1;
    public LayerMask enemyLayers;
    
    void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            animator.SetTrigger("AttackSword");
            attack();
        }
    }

    void attack()
    {
       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
       foreach (Collider2D enemy in hitEnemies)
       {
           Debug.Log("We hit and its currenntHealth ="+enemy.GetComponent<Health>().CurrentHealth);
           enemy.GetComponent<Health>().TakeDamage(damageValue);
       }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}