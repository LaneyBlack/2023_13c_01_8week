using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BossManagement : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarSliderCanvas;
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] public GameObject BossVisuals;
    [SerializeField] public GameObject DeathVfxPrefab;

    private Health _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    private int counter = 0;

    private bool _deathStarted;
    
    void Update()
    {
        updateHealthBar();
        if (_health.IsDead() && counter == 0)
        {
            counter++;
            _health.maxHealth *= 2;
            _health.RestoreHealth(_health.maxHealth);
            _healthBarSliderCanvas.transform.localScale += new Vector3(0.001f, 0, 0);
        }

        if (_health.IsDead() && counter == 1 && _deathStarted == false)
        {
            _deathStarted = true;
            BossVisuals.SetActive(false);
            
            StartCoroutine(DestroyAfterEffect());
        }
    }

    private void updateHealthBar()
    {
        _healthBarSlider.value = (float)_health.CurrentHealth / _health.maxHealth;
    }

    IEnumerator DestroyAfterEffect()
    {
        var spawnedVfx = Instantiate(DeathVfxPrefab,transform.position, Quaternion.identity);
        Destroy(spawnedVfx, 3f);
        yield return new WaitForSeconds(1f);
        // Destroy(_health.gameObject);
        Destroy(transform.parent.gameObject);
    }
}