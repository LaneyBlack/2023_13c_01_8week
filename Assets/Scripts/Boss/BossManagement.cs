using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossManagement : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarSliderCanvas;
    [SerializeField] private Slider _healthBarSlider;
    private Health _health;
    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    private int counter = 0;
    void Update()
    {
        updateHealthBar();
        if (_health.IsDead() && counter==0)
        {
            counter++;
            _health.maxHealth = 20;
            _health.RestoreHealth(_health.maxHealth);
            _healthBarSliderCanvas.transform.localScale += new Vector3(0.001f, 0, 0);

        }
        if(_health.IsDead() && counter==1)
        {
            Destroy(_health.GameObject());
        }
    }

    private void updateHealthBar()
    {
        _healthBarSlider.value = (float) _health.CurrentHealth / _health.maxHealth;
    }
}
