using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    [SerializeField] public BoxCollider2D userOfAttack;

    public void Initialize(GameObject user)
    {
        userOfAttack = user.GetComponent<BoxCollider2D>();
    }

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