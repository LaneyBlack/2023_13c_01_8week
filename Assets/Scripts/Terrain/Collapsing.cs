using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsing : MonoBehaviour
{
    [SerializeField] private float _collapseDelay;
    [SerializeField] private float _destroyDelay;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(delayCollapse());
        }
        
    }

    private IEnumerator delayCollapse()
    {
        yield return new WaitForSeconds(_collapseDelay);
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, _destroyDelay);
    }
}