using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Health playerHealth;
    public Animator _Animator;
    private bool isDead = false; 
    private bool deathAnimationStarted = false;
    private float deathTimer = 0f;
    private float deathAnimationDuration;

    private Rigidbody2D rb;
    private void Start()
    {
        playerHealth = GetComponentInParent<Health>();
        rb = GetComponentInParent<Rigidbody2D>();

        // _Animator = GetComponent<Animator>();
        if (_Animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
        }
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
            deathAnimationStarted = true;
            deathTimer = 0f; 

            AnimatorStateInfo stateInfo = _Animator.GetCurrentAnimatorStateInfo(0);
            deathAnimationDuration = stateInfo.length;
        }
        if (isDead && deathAnimationStarted)
        {
            deathTimer += Time.deltaTime; 

            if (deathTimer >= deathAnimationDuration)
            {
                //transform.parent.gameObject.SetActive(false);
            }
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
