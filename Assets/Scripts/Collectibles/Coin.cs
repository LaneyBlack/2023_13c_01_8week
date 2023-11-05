using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            InvenoryManagment.NumberOfCoins+=value;
            Debug.Log("Money: " +  InvenoryManagment.NumberOfCoins);
            Destroy(gameObject);
        }
    }
}