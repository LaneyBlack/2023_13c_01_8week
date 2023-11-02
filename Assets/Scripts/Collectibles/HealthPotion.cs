using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healthRestore = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Restore health to the player
            collision.transform.GetComponent<Health>().RestoreHealth(healthRestore);
            // Then destroy the health potion game object
            Destroy(gameObject);
        }
    }
}