using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float _damageCooldown = 1.0f;
    private float _lastDamageTime;
    private GameObject _player;
    private Health _playerHealth;
    private bool _playerIsAlive = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerHealth = _player.GetComponent<Health>();
        }
    }

    private void Update()
    {
        if (_playerHealth != null && !_playerHealth.IsDead())
        {
            _playerIsAlive = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_playerIsAlive) return;

        GameObject spikeHit = collision.gameObject;

        if (!spikeHit.CompareTag("Player") || Time.time - _lastDamageTime < _damageCooldown)
        {
            return;
        }

        if (_playerHealth == null || _playerHealth.IsDead())
        {
            return;
        }

        _playerHealth.TakeDamage(1);
        _lastDamageTime = Time.time;

        if (_playerHealth.IsDead())
        {
            _playerIsAlive = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _lastDamageTime = Time.time - _damageCooldown;
        }
    }
}