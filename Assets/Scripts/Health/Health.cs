using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth; 
    }

    //damages the player and returns true if player is dead
    public bool TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
        return IsDead();
    }

    public bool IsDead()
    {
        return CurrentHealth == 0;
    }
}
