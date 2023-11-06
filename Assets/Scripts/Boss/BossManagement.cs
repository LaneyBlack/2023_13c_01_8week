using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossManagement : MonoBehaviour
{
    private Health _health;
    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    void Update()
    {
        if (_health.IsDead())
        {
           Destroy(_health.GameObject());
        }

    }
}
