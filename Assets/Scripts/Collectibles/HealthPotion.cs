using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healthRestore = 1;
    private Health health;

    private void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    public void usePotion()
    {
        if (InvenoryManagment.NumberOfPotions > 0 && !health.atFullHealth())
        {
            health.RestoreHealth(healthRestore);
            InvenoryManagment.NumberOfPotions--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            InvenoryManagment.NumberOfPotions++;
            Destroy(gameObject);
        }
    }
}