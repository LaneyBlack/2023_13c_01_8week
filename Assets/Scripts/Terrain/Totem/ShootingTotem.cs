using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTotem : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _firePoint;
    private float _cooldownTimer;
    [SerializeField] private TrapProjectile.BulletDirection _shootDirection = TrapProjectile.BulletDirection.PositiveX;
    [Header("Bullet Pooling")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize = 10;
    private List<GameObject> _bulletPool;
    private Animator _animator;

    private void Start()
    {
        _bulletPool = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetActive(false);
            _bulletPool.Add(bullet);
        }
        
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        AdjustAnimationSpeed();
    }
    
    private void AdjustAnimationSpeed()
    {
        float animationSpeed = 1f / _attackCooldown;
        _animator.speed = animationSpeed;
    }

    private GameObject GetBullet()
    {
        foreach (var bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        GameObject newBullet = Instantiate(_bulletPrefab, transform);
        _bulletPool.Add(newBullet);
        return newBullet;
    }

    private void Awake()
    {
        if (!_firePoint)
        {
            _firePoint = transform.Find("Fire_Point");
        }
    }

    private void Attack()
    {
        _animator.SetTrigger("Shoot");
        StartCoroutine(DelayedBulletSpawn());
    }

    private IEnumerator DelayedBulletSpawn()
    {
        float delayDuration = _attackCooldown / 2; 
        yield return new WaitForSeconds(delayDuration);

        GameObject bulletInstance = GetBullet();
        bulletInstance.transform.position = _firePoint.position;
        bulletInstance.transform.rotation = _firePoint.rotation;

        TrapProjectile trapProjectile = bulletInstance.GetComponent<TrapProjectile>();
        trapProjectile.ActivateProjectile();
        trapProjectile._bulletDirection = _shootDirection;
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;
        
        if (_cooldownTimer >= _attackCooldown)
        {
            Attack();
            _cooldownTimer = 0;
        }
    }
}
