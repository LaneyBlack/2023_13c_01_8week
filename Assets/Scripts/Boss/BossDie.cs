using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    private Health bossHealth;
    [SerializeField] public GameObject GrownBossVisuals;

    [SerializeField] public GameObject SmallBossVisuals;

    [SerializeField] public Animator _SmallBossAnimator;
    private bool isDead = false;
    private bool deathAnimationStarted = false;
    private float deathTimer = 0f;
    private float deathAnimationDuration;

    private Rigidbody2D rb;

    private void Start()
    {
        bossHealth = GetComponentInParent<Health>();
        var clips = _SmallBossAnimator.runtimeAnimatorController.animationClips;
        deathAnimationDuration = clips.First(a => a.name == "Die_boss").length;
    }

    private void Update()
    {
        if (isDead)
        {
            if (deathAnimationStarted)
            {
                deathTimer += Time.deltaTime;

                if (deathTimer >= deathAnimationDuration)
                {
                    ToggleBossState(true);
                    deathAnimationStarted = false;
                }
                bossHealth.RestoreHealth(bossHealth.maxHealth);
            }
        }
        else if (bossHealth != null && bossHealth.IsDead())
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        ToggleBossState(false);
        isDead = true;
        _SmallBossAnimator.SetTrigger("BossDeathTrigger");
        deathAnimationStarted = true;
        deathTimer = 0f;
    }

    private void ToggleBossState(bool isGrown)
    {
        GrownBossVisuals.SetActive(isGrown);
        SmallBossVisuals.SetActive(isGrown == false);
    }
}