using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth; 
    }

    //damages the player and returns true if player is dead
    public bool takeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        return isDead();
    }

    public bool isDead()
    {
        return currentHealth == 0;
    }
    public void KillForTesting()
    {
        currentHealth = 0;
    }
}
