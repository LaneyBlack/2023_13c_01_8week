using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrownAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject GrownBossVisuals;

    [SerializeField] private Animator _GrownBossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] private GameObject waterProjectile;

    public LayerMask playerLayers;

    [Header("Values for preferences")] [SerializeField]
    private float attackGrownRange = 3f;

    [SerializeField] private float attackGrownCooldown = 2f; // Cooldown between attacks

    private GameObject player;
    private Health bossHealth;
    private float _cooldownTimer = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        bossHealth = GetComponentInParent<Health>();
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);

        if (GrownBossVisuals.activeSelf)
        {
            if (distanceToPlayer <= attackGrownRange && _cooldownTimer >= attackGrownCooldown)
            {
                Grown_Attack();
                _cooldownTimer = 0f;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Grown_Attack()
    {
        bossMovement.canMove = false;

        _GrownBossAnimator.SetTrigger("Attack");
        StartCoroutine(AppearProjectile());
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
}