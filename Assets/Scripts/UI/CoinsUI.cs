using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private Text coinsAmount;
    private static CoinsUI instance;

    private void Start()
    {
        coinsAmount.text = "0";
    }

    private void Awake()
    {
        instance = this;
    }

    public static CoinsUI get()
    {
        return instance;
    }

    public void setCoints(int value)
    {
        coinsAmount.text = value.ToString();
    }
}