using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int CurrentHealth { get; private set; }
    private Animator _animator;
    private static readonly int Die = Animator.StringToHash("die");
    private static readonly int TakeHit = Animator.StringToHash("takeHit");

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
            _animator.SetTrigger(TakeHit);
            if (CurrentHealth <= 0)
            {
                _animator.SetTrigger(Die);  
            }
        }catch (Exception e) {
            Debug.LogError(e);
        }
        return IsDead();
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