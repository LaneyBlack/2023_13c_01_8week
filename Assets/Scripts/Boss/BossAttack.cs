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
    [SerializeField] private GameObject waveProjectile;

    public LayerMask playerLayers;

    [Header("Values for preferences")] [SerializeField]
    private float attackGrownRange = 3f;

    [SerializeField] private float attackSmallRange = 4f;
    [SerializeField] private float attackGrownCooldown = 2f; // Cooldown between attacks
    [SerializeField] private float attackSmallCooldown = 1.5f; // Cooldown between attacks

    private GameObject player;
    private Health bossHealth;
    private float timer = 0f;
    private float _cooldownTimer = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        bossHealth = GetComponentInParent<Health>();
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;
        timer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);
        if (bossHealth.CurrentHealth < (Math.Round((float)(bossHealth.maxHealth / 2) + 1)) && timer >= 5 &&
            bossMovement.isGrounded())
        {
            StartCoroutine(DelayedAttack());
            timer = 0;
            _cooldownTimer = 0f;
        }
        else
        {
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
    }

// Coroutine for delayed attack
    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 seconds
        Small_Attack_3();
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
    void Small_Attack_3()
    {
        bossMovement.canMove = false;
        _SmallBossAnimator.SetTrigger("AttackSmall3");
        StartCoroutine(AppearWave());
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
        }
        else
        {
            bubbleProjectile.transform.localPosition = new Vector3(-0.08f, -0.09f, 0);
        }

        while (time < 1f)
        {
            if (isFlip)
            {
                bubbleProjectile.transform.localPosition += new Vector3(0.005f, 0, 0);
            }
            else
            {
                bubbleProjectile.transform.localPosition += new Vector3(-0.005f, 0, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        bubbleProjectile.transform.localScale = originalScale;
        bubbleProjectile.SetActive(false);
        bossMovement.canMove = true;
    }

    private IEnumerator AppearWave()
    {
        yield return new WaitForSeconds(0.3f);
        waveProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = waveProjectile.transform.localScale;
        bool isFlip = (transform.parent.position.x - player.transform.position.x) < 0;
        if (isFlip)
        {
            waveProjectile.GetComponent<WaterProjectile>().changePosition(0, true);

            waveProjectile.transform.localPosition = new Vector3(0.35f, 0.071f, 0); //prawo
        }
        else
        {
            waveProjectile.GetComponent<WaterProjectile>().changePosition(0, false);

            waveProjectile.transform.localPosition = new Vector3(-0.35f, 0.071f, 0);
        }

        while (time < 2f)
        {
            if (isFlip)
            {
                waveProjectile.transform.localPosition += new Vector3(0.01f, 0, 0);
            }
            else
            {
                waveProjectile.transform.localPosition += new Vector3(-0.01f, 0, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        waveProjectile.transform.localScale = originalScale;
        waveProjectile.SetActive(false);
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