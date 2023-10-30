using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapProjectile : ProjectileDamage
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    private float _lifetime;
   
    public void ActivateProjectile()
    {
        _lifetime = 0;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        float _movementSpeed = _speed * Time.deltaTime;
        transform.Translate(_movementSpeed, 0, 0);

        _lifetime += Time.deltaTime;
        if (_lifetime > _resetTime)
        {
            ResetProjectile();
        }
    }
    private void ResetProjectile()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet")) return;
        
        base.OnTriggerEnter2D(col);
        ResetProjectile();
    }


}
