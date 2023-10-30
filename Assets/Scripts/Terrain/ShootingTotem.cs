using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTotem : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _bullet;
    [SerializeField] private GameObject[] _bullets;
    private float _cooldownTimer;

    private void Attack()
    {
        _cooldownTimer = 0;

        int bulletIndex = FindBullet();
        _bullets[bulletIndex].transform.position = _bullet.position;
        _bullets[bulletIndex].GetComponent<TrapProjectile>().ActivateProjectile();
    }


    private int FindBullet()
    {
        for (int i = 0; i < _bullets.Length; i++)
        {
            if (!_bullets[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;
        
        if (_cooldownTimer >= _attackCooldown)
        {
            Attack();
        }
    }
}
