using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    private TextMeshProUGUI coinsAmount;

    private void Start()
    {
        coinsAmount = GameObject.FindGameObjectWithTag("UI_CAmount").GetComponent<TextMeshProUGUI>();
        coinsAmount.text = "0";
    }

    private void Update()
    {
        coinsAmount.text = InvenoryManagment.NumberOfCoins.ToString();
    }

    //private void Awake()
    //{
    //    instance = this;
    //}

    //public static CoinsUI get()
    //{
    //    return instance;
    //}

    //public void setCoints(int value)
    //{
    //    coinsAmount.text = value.ToString();
    //}
}
