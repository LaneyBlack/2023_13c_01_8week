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

    void heal()
    {
        health.RestoreHealth(healthRestore);
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