using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossManagement : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    private Health _health;
    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    void Update()
    {
        updateHealthBar();
        if (_health.IsDead())
        {
           Destroy(_health.GameObject());
        }
    }

    private void updateHealthBar()
    {
        _healthBarSlider.value = (float) _health.CurrentHealth / _health.maxHealth;
    }
}
