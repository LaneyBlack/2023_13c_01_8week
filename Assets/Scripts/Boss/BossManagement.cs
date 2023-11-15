using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossManagement : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarSliderCanvas;
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] public GameObject BossVisuals;

    private Health _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    private int counter = 0;

    void Update()
    {
        updateHealthBar();
        if (_health.IsDead() && counter == 0)
        {
            counter++;
            _health.maxHealth = 20;
            _health.RestoreHealth(_health.maxHealth);
            _healthBarSliderCanvas.transform.localScale += new Vector3(0.001f, 0, 0);
        }

        if (_health.IsDead() && counter == 1)
        {
            GameObject effect = ParticleSystemPool.Instance.GetParticle();
            BossVisuals.SetActive(false);
            effect.transform.position = transform.position;
            effect.transform.rotation = Quaternion.identity;
            StartCoroutine(DestroyAfterEffect(effect));
        }
    }

    private void updateHealthBar()
    {
        _healthBarSlider.value = (float)_health.CurrentHealth / _health.maxHealth;
    }

    IEnumerator DestroyAfterEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1f); 
        Destroy(_health.gameObject);
        ParticleSystemPool.Instance.ReturnParticle(effect);
    }
}