using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Health playerHealth;
    private Animator _Animator;
    private Rigidbody2D rb;
    private bool isDead = false;

    public Behaviour[] components;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();

        _Animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
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
        _Animator.ResetTrigger("takeHit");
        _Animator.SetTrigger("Die");

        rb.bodyType = RigidbodyType2D.Static;

        foreach (var component in components)
            component.enabled = false;
    }

    public void handleRespawn()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        foreach (var component in components)
            component.enabled = true;

        playerHealth.RestoreHealth(playerHealth.maxHealth);
        _Animator.ResetTrigger("Die");
        _Animator.Play("Idle");
        isDead = false;
    }
}
