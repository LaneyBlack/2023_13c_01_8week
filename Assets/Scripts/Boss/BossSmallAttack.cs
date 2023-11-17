using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSmallAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject bossVisuals;

    [SerializeField] private Animator bossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private GameObject bubbleProjectile;
    [SerializeField] private GameObject waveProjectile;


    public LayerMask playerLayers;

    [Header("Values for preferences")] [SerializeField, Range(0f, 16f)]
    private float bubbleAttackRange = 8f;

    [SerializeField, Range(0f, 6f)] private float wandAttackRange = 1f;
    [SerializeField, Range(0f, 3f)] private float bubbleAttackCooldown = 1.5f;
    [SerializeField, Range(0f, 3f)] private float wandAttackCooldown = 1f;
    [SerializeField, Range(0.5f, 10f)] private float waveAttackCooldown = 5f;
    [SerializeField, Range(0, 20)] private int specialBossHealthLimitAttack = 15;

    private AttackBubble _attackBubble;
    private AttackWave _attackWave;
    private AttackBubbleWand _attackBubbleWand;

    private GameObject _player;
    private Health _bossHealth;
    private float _cooldownTimer = 0;
    private float _cooldownTimerWave = 0;
    public static bool IsSmallAttackFinished = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        IsSmallAttackFinished = true;
        _bossHealth = GetComponentInParent<Health>();
        _attackBubble = new AttackBubble(bubbleProjectile, transform.parent, _player, bossMovement);
        _attackWave = new AttackWave(waveProjectile, transform.parent, _player, bossMovement);
        _attackBubbleWand = new AttackBubbleWand(bossMovement, attackPoint, playerLayers, 1.4f);
        _cooldownTimer = 0;
        _cooldownTimerWave = 0;
    }

    private void Update()
    {
        if (bossVisuals.activeSelf && IsSmallAttackFinished)
        {
            TimerCounter();
            float distanceToPlayer = Vector2.Distance(transform.parent.position, _player.transform.position);
            if (_cooldownTimerWave >= waveAttackCooldown && bossMovement.isGrounded())
            {
                PerformAttack("Attack3", () => _attackWave.AppearWave(1f, 1.58f));
                _cooldownTimerWave = 0f;
                _cooldownTimer = 0f;
            }

            if (_cooldownTimer >= wandAttackCooldown && distanceToPlayer <= wandAttackRange)
            {
                _attackBubbleWand.SmallAttackColliders();
                PerformAttack("Attack",
                    () => _attackBubbleWand.SmallAttackAnimationDuration(0.3f));
                _cooldownTimer = 0f;
            }

            if (_cooldownTimer >= bubbleAttackCooldown && distanceToPlayer <= bubbleAttackRange)
            {
                PerformAttack("Attack2", () => _attackBubble.AppearBubble(0.3f, 1f));

                _cooldownTimer = 0f;
            }
        }

        void PerformAttack(string attackTrigger, Func<IEnumerator> attackBehaviorCoroutine)
        {
            IsSmallAttackFinished = false;
            bossMovement.canMove = false;
            bossAnimator.SetTrigger(attackTrigger);
            StartCoroutine(attackBehaviorCoroutine());
        }

        void TimerCounter()
        {
            _cooldownTimer += Time.deltaTime;
            if (_bossHealth.CurrentHealth < specialBossHealthLimitAttack)
            {
                _cooldownTimerWave += Time.deltaTime;
            }
        }
    }
}