using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossGrownAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject bossVisuals;

    [SerializeField] private Animator bossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] private GameObject waterProjectile;
    [SerializeField] private GameObject rainbowProjectile;

    [Header("Values for preferences")] [SerializeField]
    private float attackRange = 3f;

    [SerializeField, Range(0f, 5f)] private float waterProjectileAttackCooldown = 1.5f; 
    [SerializeField, Range(0f, 10f)] private float rainbowAttackCooldown = 5f;

    [SerializeField, Range(0, 20)] private int specialBossHealthLimitAttack = 10;

    private GameObject _player;
    private Health _bossHealth;
    private float _cooldownTimerRainbow = 0f;
    private float _cooldownTimer = 0;
    public static bool IsGrownAttackFinished = true;
    public AttackWaterProjectile _attackWaterProjectile;
    public AttackRainbow _attackRainbow;

    private void Start()
    {
        IsGrownAttackFinished = true;
        _cooldownTimer = 0;
        _cooldownTimerRainbow = 0;
        _player = GameObject.FindGameObjectWithTag("Player");
        _bossHealth = GetComponentInParent<Health>();
        _attackWaterProjectile = new AttackWaterProjectile(waterProjectile, transform.parent, _player, bossMovement);
        _attackRainbow = new AttackRainbow(rainbowProjectile, bossMovement);
    }

    private void Update()
    {
        if (bossVisuals.activeSelf && IsGrownAttackFinished)
        {
            TimerCounter();

            float distanceToPlayer = Vector2.Distance(transform.parent.position, _player.transform.position);
            if (_cooldownTimerRainbow >= rainbowAttackCooldown && bossMovement.isGrounded())
            {
                PerformAttack("Attack2", () => _attackRainbow.AppearRainbow(0.6f, 1.6f));

                _cooldownTimerRainbow = 0;
                _cooldownTimer = 0f;
            }
            else if (_cooldownTimer >= waterProjectileAttackCooldown && distanceToPlayer <= attackRange)
            {
                PerformAttack("Attack", () => _attackWaterProjectile.AppearWaterProjectile(0.3f, 1f));

                _cooldownTimer = 0f;
            }
        }
    }

    void PerformAttack(string attackTrigger, Func<IEnumerator> attackBehaviorCoroutine)
    {
        IsGrownAttackFinished = false;
        bossMovement.canMove = false;
        bossAnimator.SetTrigger(attackTrigger);
        StartCoroutine(attackBehaviorCoroutine());
    }

    void TimerCounter()
    {
        _cooldownTimer += Time.deltaTime;
        if (_bossHealth.CurrentHealth < specialBossHealthLimitAttack)
        {
            _cooldownTimerRainbow += Time.deltaTime;
        }
    }
}