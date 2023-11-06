using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; 
    // private BoxCollider2D _boxCollider2D;

    void Awake()
    {
        // _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Health health = collider.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}

