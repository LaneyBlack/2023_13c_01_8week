using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BossSmallAttack : MonoBehaviour
{
    [Header("Objects to attach")] [SerializeField]
    public GameObject SmallBossVisuals;

    [SerializeField] private Animator _SmallBossAnimator;
    [SerializeField] private BossMovement bossMovement;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private GameObject bubbleProjectile;
    [SerializeField] private GameObject waveProjectile;

   
    public LayerMask playerLayers;

    [FormerlySerializedAs("attackSmallRange")] [Header("Values for preferences")] [SerializeField]
    private float attackRange = 4f;

    [SerializeField] private float attackCooldown = 1.5f; // Cooldown between attacks

    private AttackBubble attackBubble;
    
    private AttackWave attackWave;
    private AttackBubbleWand attackBubbleWand;
    
    private GameObject player;
    private Health bossHealth;
    private float timer = 0f;
    private float _cooldownTimer = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        bossHealth = GetComponentInParent<Health>();
        attackBubble = new AttackBubble(bubbleProjectile, transform.parent, player, bossMovement);
        attackWave = new AttackWave(waveProjectile, transform.parent, player, bossMovement);
        attackBubbleWand = new AttackBubbleWand(bossMovement, attackPoint, playerLayers);


    }

    private void Update()
    {
        if (SmallBossVisuals.activeSelf)
        {
            _cooldownTimer += Time.deltaTime;
            timer += Time.deltaTime;

            float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);
            if (bossHealth.CurrentHealth < (Math.Round((float)(bossHealth.maxHealth / 2) + 1)) && timer >= 5 &&
                bossMovement.isGrounded())
            {
                Small_Attack_3();
                timer = 0;
                _cooldownTimer = 0f;
            }
            else
            {
                if (distanceToPlayer <= attackRange / 4 && _cooldownTimer >= attackCooldown)
                {
                    Small_Attack();
                    _cooldownTimer = 0f;
                }
                else if (distanceToPlayer <= attackRange && _cooldownTimer >= attackCooldown)
                {
                    Small_Attack_2();
                    _cooldownTimer = 0f;
                }
            }
        }
    }


    void Small_Attack()
    {
        bossMovement.canMove = false;

        _SmallBossAnimator.SetTrigger("Attack");
        // SmallAttackColliders();
        // StartCoroutine(SmallAttackAnimationDuration());
        
        attackBubbleWand.SmallAttackColliders();
        StartCoroutine(attackBubbleWand.SmallAttackAnimationDuration());
    }

    void Small_Attack_2()
    {
        bossMovement.canMove = false;
        _SmallBossAnimator.SetTrigger("AttackSmall2");
        StartCoroutine(attackBubble.AppearBubble());
    }

    void Small_Attack_3()
    {
        bossMovement.canMove = false;
        _SmallBossAnimator.SetTrigger("AttackSmall3");
        StartCoroutine(attackWave.AppearWave());
    }
}