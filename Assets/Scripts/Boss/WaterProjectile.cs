using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    [SerializeField] public BoxCollider2D userOfAttack;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == userOfAttack.gameObject)
        {
            return;
        }

        Health health = collider.GetComponent<Health>();
        health.TakeDamage(damageAmount);
    }
}