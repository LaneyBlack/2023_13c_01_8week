using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private bool hit;
    void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        hit = true;
    }
}
