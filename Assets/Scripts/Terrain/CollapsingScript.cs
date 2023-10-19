using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsing : MonoBehaviour
{
    [SerializeField] private float _collapseDelay;
    [SerializeField] private float _destroyDelay;
    [SerializeField] private float _respawnDelay = 5.0f;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    private Vector3 _initialPosition;
    private bool _isCollapsed = false;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isCollapsed)
        {
            StartCoroutine(delayCollapse());
        }
    }
    
    private IEnumerator delayCollapse()
    {
        _isCollapsed = true;
        yield return new WaitForSeconds(_collapseDelay);
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(DelayedDeactivate());
    }

    private IEnumerator DelayedDeactivate()
    {
        yield return new WaitForSeconds(_destroyDelay);
        gameObject.SetActive(false);
        CollapsingPlatformManager.Instance.RespawnPlatform(gameObject, _initialPosition, _respawnDelay);
    }
    
    public void ResetPlatform()
    {
        _isCollapsed = false;
    }

}