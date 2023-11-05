using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : ProjectileDamage
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    private float _lifetime;
    private Animator _bulletAnimator;
    private bool _isActive = false;
    private bool _hasHit = false;

    public enum BulletDirection
    {
        PositiveX,
        NegativeX
    }

    public BulletDirection _bulletDirection = BulletDirection.PositiveX;

    private void Awake()
    {
        _bulletAnimator = GetComponent<Animator>();
    }

    public void ActivateProjectile()
    {
        if (!_bulletAnimator)
        {
            _bulletAnimator = GetComponent<Animator>();
            if (!_bulletAnimator)
            {
                Debug.LogError("Animator not found on the TrapProjectile!");
                return;
            }
        }

        _lifetime = 0;
        _isActive = true;
        _hasHit = false;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_isActive)
        {
            float _movementSpeed = (_bulletDirection == BulletDirection.PositiveX ? _speed : -_speed) * Time.deltaTime;
            transform.Translate(_movementSpeed, 0, 0);
        }

        _lifetime += Time.deltaTime;
        if (_lifetime > _resetTime)
        {
            StartCoroutine(ResetAfterAnimation(0.5f));
        }
    }

    IEnumerator ResetAfterAnimation(float delay)
    {
        _bulletAnimator.SetTrigger("Break");
        yield return new WaitForSeconds(delay);
        CompleteReset();
    }

    private void CompleteReset()
    {
        _bulletAnimator.Rebind();

        _isActive = false;
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Totem") || col.CompareTag("Bullet")) return;

        if (!_hasHit)
        {
            base.OnTriggerEnter2D(col);
            _isActive = false;
            StartCoroutine(ResetAfterAnimation(0.5f));
            _hasHit = true;
        }
    }

}
