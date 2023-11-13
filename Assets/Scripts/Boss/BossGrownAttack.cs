using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossGrownAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject GrownBossVisuals;

    [SerializeField] private Animator _GrownBossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] private GameObject waterProjectile;
    [SerializeField] private GameObject rainbowProjectile;

    [Header("Values for preferences")] [SerializeField]
    private float attackRange = 3f;

    [SerializeField] private float attackCooldown = 2f; // Cooldown between attacks

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
        
        if (GrownBossVisuals.activeSelf)
        {
            _cooldownTimer += Time.deltaTime;
            timer += Time.deltaTime;
            float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);
            if (bossHealth.CurrentHealth < (Math.Round((float)(bossHealth.maxHealth / 2) + 1)) && timer >= 5 &&
                bossMovement.isGrounded())
            {
                Attack2();
                timer = 0;
                _cooldownTimer = 0f;
            }
            else if (distanceToPlayer <= attackRange && _cooldownTimer >= attackCooldown)
            {
                Attack();
                _cooldownTimer = 0f;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Attack()
    {
        bossMovement.canMove = false;

        _GrownBossAnimator.SetTrigger("Attack");
        StartCoroutine(AppearProjectile());
    }
    void Attack2()
    {
        bossMovement.canMove = false;
        _GrownBossAnimator.SetTrigger("Attack2");
        StartCoroutine(AppearRainbow());
    }
    // ReSharper disable Unity.PerformanceAnalysis
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
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator AppearRainbow()
    {
        yield return new WaitForSeconds(1.3f);
        rainbowProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = rainbowProjectile.transform.localScale;
        rainbowProjectile.transform.localPosition = new Vector3(0, 0.1f, 0); 

        while (time < 2f)
        {
            rainbowProjectile.transform.localScale += new Vector3(0.001f, 0.0014f, 0);
            rainbowProjectile.transform.localPosition += new Vector3(0, 0.000075f, 0);
            // rainbowProjectile. += new Vector3(0.007f, 0.02f, 0);
            yield return null;
            time += Time.deltaTime;
        }

        rainbowProjectile.transform.localScale = originalScale;
        rainbowProjectile.SetActive(false);
        bossMovement.canMove = true;
    }
}