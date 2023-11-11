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
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f; // Cooldown between attacks
    [SerializeField] private BossMovement bossMovement;


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
        if (distanceToPlayer <= attackRange && _cooldownTimer >= attackCooldown)
        {
            Attack();
            _cooldownTimer = 0f;
        }
    }

    void Attack()
    {
        bossMovement.canMove = false;
        if (SmallBossVisuals.activeSelf)
        {
            _SmallBossAnimator.SetTrigger("Attack");
            bossMovement.canMove = true;
        }
        else
        {
            _GrownBossAnimator.SetTrigger("Attack");
            StartCoroutine(AppearProjectile());
        }
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
            waterProjectile.transform.localScale +=  new Vector3(0.008f, 0, 0); // Modify this as needed
            waterProjectile.GetComponent<SpriteRenderer>().flipX = isFlip;
            if (isFlip)
            {
                waterProjectile.transform.localPosition = new Vector3(0.1f, 0.02f, 0);
            }
            else
            {
                waterProjectile.transform.localPosition = new Vector3(-0.1f, 0.02f, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        waterProjectile.transform.localScale = originalScale;
        waterProjectile.SetActive(false);
        bossMovement.canMove = true;
    }
}