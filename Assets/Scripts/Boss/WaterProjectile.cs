using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    [SerializeField] public BoxCollider2D userOfAttack;
    [SerializeField, Range(0f,40f)] public float HitForce = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == userOfAttack.gameObject)
        {
            return;
        }

        if (collider.GetComponent<Health>() != null)
        {
            collider.GetComponent<Health>().TakeDamage(damageAmount);

            collider.GetComponent<Rigidbody2D>()
                .AddForce(
                    new Vector2(HitForce * (collider.GetComponent<SpriteRenderer>().flipX ? 2 : -2), HitForce),
                    ForceMode2D.Impulse);
        }
    }

    public void changePosition(float x, bool IsFlip)
    {
        transform.localPosition = new Vector3(x, 0, 0);
        GetComponent<SpriteRenderer>().flipX = IsFlip;
    }
}