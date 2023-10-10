using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPount : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _cooldown;
    
    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Instantiate(_projectilePrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(_cooldown);
        }
    }
}
