using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Health playerHealth;
    private Animator _Animator;
    private bool isDead = false;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();

        _Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerHealth.KillForTesting();
        }

        if (!isDead && playerHealth != null && playerHealth.IsDead())
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        isDead = true; 
        _Animator.SetTrigger("Die");
    }

    public void handleRespawn()
    {
        playerHealth.RestoreHealth(playerHealth.maxHealth);
        _Animator.ResetTrigger("Die");
        _Animator.Play("Idle");
        isDead = false;
    }
}
