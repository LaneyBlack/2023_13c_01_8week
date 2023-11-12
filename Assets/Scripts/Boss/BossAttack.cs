using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] public GameObject SmallBossVisuals;
    [SerializeField] private Animator _SmallBossAnimator;
    [SerializeField] private Animator _GrownBossAnimator;

    [SerializeField] private GameObject waterProjectile;
    private GameObject player;
    [SerializeField] private float attackGrownRange = 3f;
    [SerializeField] private float attackSmallRange = 4f;
    [SerializeField] private float attackGrownCooldown = 2f; // Cooldown between attacks
    [SerializeField] private float attackSmallCooldown = 1.5f; // Cooldown between attacks
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] public Transform attackPoint;

    public LayerMask playerLayers;

    private float
        _cooldownTimer = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);
        if (SmallBossVisuals.activeSelf)
        {
            if (distanceToPlayer <= attackSmallRange/4 && _cooldownTimer >= attackSmallCooldown)
            {
                Attack();
                _cooldownTimer = 0f;
            }
            // else if (distanceToPlayer <= attackSmallRange && _cooldownTimer >= attackSmallCooldown)
            // {
            //     Small_Attack_2();
            //     _cooldownTimer = 0f;
            // }
        }
        else
        {
            if (distanceToPlayer <= attackGrownRange && _cooldownTimer >= attackGrownCooldown)
            {
                Attack();
                _cooldownTimer = 0f;
            }
        }
    }

    // void Small_Attack_2()
    // {
    //     bossMovement.canMove = false;
    //     _SmallBossAnimator.SetTrigger("Attack_small_boss");
    //     StartCoroutine(AppearBubble());
    // }

    // ReSharper disable Unity.PerformanceAnalysis
    void Attack()
    {
        bossMovement.canMove = false;
        if (SmallBossVisuals.activeSelf)
        {
            _SmallBossAnimator.SetTrigger("Attack");
            SmallAttackColliders();
            StartCoroutine(SmallAttackAnimationDuration());
        }
        else
        {
            _GrownBossAnimator.SetTrigger("Attack");
            StartCoroutine(AppearProjectile());
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // private IEnumerator AppearBubble()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     waterProjectile.SetActive(true);
    //     float time = 0;
    //     Vector3 originalScale = waterProjectile.transform.localScale;
    //
    //     while (time < 1f)
    //     {
    //         bool isFlip = (transform.parent.position.x - player.transform.position.x) < 0;
    //         waterProjectile.transform.localScale += new Vector3(0.007f, 0, 0);
    //         waterProjectile.GetComponent<SpriteRenderer>().flipX = isFlip;
    //         if (isFlip)
    //         {
    //             waterProjectile.transform.localPosition = new Vector3(-0.07f, 0, 0); //prawo
    //
    //             waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(0.15f, true);
    //         }
    //         else
    //         {
    //             waterProjectile.transform.localPosition = new Vector3(0.04f, 0, 0);
    //             waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(-0.15f, false);
    //         }
    //
    //         yield return null;
    //         time += Time.deltaTime;
    //     }
    //
    //     waterProjectile.transform.localScale = originalScale;
    //     waterProjectile.SetActive(false);
    //     bossMovement.canMove = true;
    // }

    private IEnumerator AppearProjectile()
    {
        yield return new WaitForSeconds(0.3f);
        waterProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = waterProjectile.transform.localScale;

        while (time < 1f)
        {
            bool isFlip = (transform.parent.position.x - player.transform.position.x) < 0;
            waterProjectile.transform.localScale += new Vector3(0.007f, 0, 0);
            waterProjectile.GetComponent<SpriteRenderer>().flipX = isFlip;
            if (isFlip)
            {
                waterProjectile.transform.localPosition = new Vector3(-0.07f, 0, 0); //prawo

                waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(0.15f, true);
            }
            else
            {
                waterProjectile.transform.localPosition = new Vector3(0.04f, 0, 0);
                waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(-0.15f, false);
            }

            yield return null;
            time += Time.deltaTime;
        }

        waterProjectile.transform.localScale = originalScale;
        waterProjectile.SetActive(false);
        bossMovement.canMove = true;
    }

    private IEnumerator SmallAttackAnimationDuration()
    {
        yield return new WaitForSeconds(0.5f);

        bossMovement.canMove = true;
    }

    void SmallAttackColliders()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1.4f, playerLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit and its currenntHealth =" + enemy.GetComponent<Health>().CurrentHealth);
            enemy.GetComponent<Health>().TakeDamage(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, 1.4f);
    }
}