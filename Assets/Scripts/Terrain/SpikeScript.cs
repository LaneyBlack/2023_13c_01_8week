using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private float lastDamageTime;
    private float damageCooldown = 1.0f;
    private bool playerIsAlive = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!playerIsAlive) return;

        GameObject spikeHit = collision.gameObject;
        
        if (!spikeHit.CompareTag("Player") || Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }
        
        Health playerHealth = spikeHit.GetComponent<Health>();
        
        if (playerHealth == null || playerHealth.isDead())
        {
            return;
        }
        
        playerHealth.takeDamage(1);

        lastDamageTime = Time.time;
        
        if (playerHealth.isDead())
        {
            playerIsAlive = false; }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lastDamageTime = Time.time - damageCooldown;
        }
    }
}
