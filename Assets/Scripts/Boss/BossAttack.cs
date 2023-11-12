using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject SmallBossVisuals;

    [SerializeField] private Animator _SmallBossAnimator;
    [SerializeField] private Animator _GrownBossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private GameObject waterProjectile;
    [SerializeField] private GameObject bubbleProjectile;
    public LayerMask playerLayers;

    [Header("Values for preferences")] [SerializeField]
    private float attackGrownRange = 3f;

    [SerializeField] private float attackSmallRange = 4f;
    [SerializeField] private float attackGrownCooldown = 2f; // Cooldown between attacks
    [SerializeField] private float attackSmallCooldown = 1.5f; // Cooldown between attacks

    private GameObject player;

    private float _cooldownTimer = Mathf.Infinity;

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
            if (distanceToPlayer <= attackSmallRange / 4 && _cooldownTimer >= attackSmallCooldown)
            {
                Small_Attack();
                _cooldownTimer = 0f;
            }
            else if (distanceToPlayer <= attackSmallRange && _cooldownTimer >= attackSmallCooldown)
            {
                Small_Attack_2();
                _cooldownTimer = 0f;
            }
        }
        else
        {
            if (distanceToPlayer <= attackGrownRange && _cooldownTimer >= attackGrownCooldown)
            {
                Grown_Attack();
                _cooldownTimer = 0f;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Small_Attack()
    {
        bossMovement.canMove = false;

        _SmallBossAnimator.SetTrigger("Attack");
        SmallAttackColliders();
        StartCoroutine(SmallAttackAnimationDuration());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Small_Attack_2()
    {
        bossMovement.canMove = false;
        _SmallBossAnimator.SetTrigger("AttackSmall2");
        StartCoroutine(AppearBubble());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Grown_Attack()
    {
        bossMovement.canMove = false;

        _GrownBossAnimator.SetTrigger("Attack");
        StartCoroutine(AppearProjectile());
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

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator AppearBubble()
    {
        yield return new WaitForSeconds(0.3f);
        bubbleProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = bubbleProjectile.transform.localScale;
        bool isFlip = (transform.parent.position.x - player.transform.position.x) < 0;

        if (isFlip)
        {
            bubbleProjectile.transform.localPosition = new Vector3(0.05f, -0.09f, 0); //prawo

            // bubbleProjectile.GetComponent<WaterProjectile>().changePosition(0.15f, true);
        }
        else
        {
            bubbleProjectile.transform.localPosition = new Vector3(-0.08f, -0.09f, 0);
            // bubbleProjectile.GetComponent<WaterProjectile>().changePosition(-0.15f, false);
        }

        while (time < 1f)
        {
            // bubbleProjectile.transform.localScale += new Vector3(0.007f, 0, 0);
            if (isFlip)
            {
                bubbleProjectile.transform.localPosition += new Vector3(0.005f, 0, 0);
            }
            else
            {
                bubbleProjectile.transform.localPosition += new Vector3(-0.005f, 0, 0);

            }

            // bubbleProjectile.GetComponent<SpriteRenderer>().flipX = isFlip;


            yield return null;
            time += Time.deltaTime;
        }

        bubbleProjectile.transform.localScale = originalScale;
        bubbleProjectile.SetActive(false);
        bossMovement.canMove = true;
    }

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
}