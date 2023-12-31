using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector] public int CurrentHealth { get; private set; }
    private Animator _animator;
    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int TakeHit = Animator.StringToHash("takeHit");

    private bool attachedToPlayer;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
    }

    //damages the player and returns true if player is dead
    public bool TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
        try {
            if (CurrentHealth <= 0)
            {
                _animator.SetBool(Die, true);

                if (tag != "Player")
                    _animator.SetTrigger(TakeHit);
            }
            else
                _animator.SetTrigger(TakeHit);
        }catch (Exception e) {
            Debug.LogError(e);
        }
        return IsDead();
    }

    public bool atFullHealth()
    {
        return CurrentHealth == maxHealth;
    }

    public bool IsDead()
    {
        return CurrentHealth == 0;
    }
    public void KillForTesting()
    {
        CurrentHealth = 0;
    }
    public void RestoreHealth(int amount)
    {
        if (CurrentHealth < maxHealth)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        }
    }
}