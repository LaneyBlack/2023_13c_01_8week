using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth; 
    }

    //damages the player and returns true if player is dead
    bool takeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        return isDead();
    }

    bool isDead()
    {
        return currentHealth == 0;
    }
}
