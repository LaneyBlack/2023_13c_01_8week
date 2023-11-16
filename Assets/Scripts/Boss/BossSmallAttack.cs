using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSmallAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject smallBossVisuals;

    [SerializeField] private Animator smallBossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private GameObject bubbleProjectile;
    [SerializeField] private GameObject waveProjectile;


    public LayerMask playerLayers;

    [Header("Values for preferences")] [SerializeField, Range(0f, 6f)]
    private float attackRange = 4f;

    [SerializeField, Range(0f, 3f)] private float attackCooldown = 1.5f;
    [SerializeField, Range(0.5f, 10f)] private float timerForWaveAttackCooldown = 4.5f;
    [SerializeField, Range(0, 10)] private int specialBossHealthLimitAttack = 4;

    private AttackBubble _attackBubble;
    private AttackWave _attackWave;
    private AttackBubbleWand _attackBubbleWand;

    private GameObject _player;
    private Health _bossHealth;
    private float _timerForWaveAttack = 0f;
    private float _cooldownTimer = Mathf.Infinity;
    public static bool IsAttackFinished = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        IsAttackFinished = true;
        _bossHealth = GetComponentInParent<Health>();
        _attackBubble = new AttackBubble(bubbleProjectile, transform.parent, _player, bossMovement);
        _attackWave = new AttackWave(waveProjectile, transform.parent, _player, bossMovement);
        _attackBubbleWand = new AttackBubbleWand(bossMovement, attackPoint, playerLayers, 1.4f);
    }

    private void Update()
    {
        if (smallBossVisuals.activeSelf && IsAttackFinished)
        {
            _cooldownTimer += Time.deltaTime;
            _timerForWaveAttack += Time.deltaTime;

            float distanceToPlayer = Vector2.Distance(transform.parent.position, _player.transform.position);
            if (_bossHealth.CurrentHealth < specialBossHealthLimitAttack &&
                _timerForWaveAttack >= timerForWaveAttackCooldown && bossMovement.isGrounded())
            {
                PerformAttack("AttackSmall3", () => _attackWave.AppearWave(1f, 2.4f));
                _timerForWaveAttack = 0;
                _cooldownTimer = 0f;
            }
            else
            {
                if (_cooldownTimer >= attackCooldown)
                {
                    if (distanceToPlayer <= attackRange / 4)
                    {
                        _attackBubbleWand.SmallAttackColliders();
                        PerformAttack("Attack",
                            () => _attackBubbleWand.SmallAttackAnimationDuration(0.5f));
                    }
                    else if (distanceToPlayer <= attackRange)
                    {
                        PerformAttack("AttackSmall2", () => _attackBubble.AppearBubble(0.3f, 1f));
                    }

                    _cooldownTimer = 0f;
                }
            }
        }
    }

    void PerformAttack(string attackTrigger, Func<IEnumerator> attackBehaviorCoroutine)
    {
        IsAttackFinished = false;
        bossMovement.canMove = false;
        smallBossAnimator.SetTrigger(attackTrigger);
        StartCoroutine(attackBehaviorCoroutine());
    }
}