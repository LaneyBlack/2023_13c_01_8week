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

        if (collider.GetComponent<Health>() != null)
        {
            collider.GetComponent<Health>().TakeDamage(damageAmount);

        }
    }

    public void changePosition(float x, bool IsFlip)
    {
        transform.localPosition = new Vector3(x, 0, 0);
        GetComponent<SpriteRenderer>().flipX = IsFlip;
    }
}