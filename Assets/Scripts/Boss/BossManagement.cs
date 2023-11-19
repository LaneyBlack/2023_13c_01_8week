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
    [FormerlySerializedAs("keyPrefab")] [SerializeField] private GameObject keyBossPrefab;
    [Header("Jump")] [SerializeField] private float keyBossForce = 20f;

    [Header("Values for preferences")] [SerializeField, Range(0f, 1f)]
    private Vector2 spawnOffsetKey = new Vector2(1f, 0f);

    [SerializeField] private int keyId = 100;

    private Health _health;

    private bool isChangeFormHappened = false;

    public static bool _deathStarted;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    void Update()
    {
        updateHealthBar();
        if (_health.IsDead() && !isChangeFormHappened)
        {
            healthBarUpdate();
            isChangeFormHappened = true;
        }

        if (_health.IsDead() && isChangeFormHappened && _deathStarted == false)
        {
            _deathStarted = true;
            BossVisuals.SetActive(false);
            SpawnKey();
            // StartCoroutine(SpawnKey());
            StartCoroutine(DestroyAfterEffect());
        }
    }

    private void healthBarUpdate()
    {
        _health.maxHealth *= 2;
        _health.RestoreHealth(_health.maxHealth);
        _healthBarSliderCanvas.transform.localScale += new Vector3(0.001f, 0, 0);
    }

    private void updateHealthBar()
    {
        _healthBarSlider.value = (float)_health.CurrentHealth / _health.maxHealth;
    }

    IEnumerator DestroyAfterEffect()
    {
        var spawnedVfx = Instantiate(DeathVfxPrefab, transform.position, Quaternion.identity);
        Destroy(spawnedVfx, 3f);
        Destroy(_health.gameObject);
        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
    }

    private void SpawnKey()
    {
        spawnOffsetKey.x = Random.Range(-1f, 1f);

        Vector3 keyPosition = transform.position + new Vector3(spawnOffsetKey.x, 1.5f, 0f);
        GameObject key = Instantiate(keyBossPrefab, keyPosition, Quaternion.identity);
        key.GetComponentInChildren<KeyBoss>().id = keyId;

        Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(1, 2f)).normalized * keyBossForce;

        key.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

}