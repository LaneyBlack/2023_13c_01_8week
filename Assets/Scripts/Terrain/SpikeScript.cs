using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject spikeHit = collision.gameObject;
        if (spikeHit.CompareTag("Player"))
        {
            //TODO: Player loses HP or dies
        }
    }
}
