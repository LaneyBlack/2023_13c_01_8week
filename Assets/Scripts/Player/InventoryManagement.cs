using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenoryManagment : MonoBehaviour
{
    public static bool IsSwordUpgraded = false; 
    public static int NumberOfCoins = 0;
    public static int NumberOfPotions = 0;
    public static List<int> KeysId = new List<int>();

    private void Start()
    {
        //Debug.Log("start of InvenoryManagment");
        //_health = GetComponentInParent<Health>();
        IsSwordUpgraded = false;
        //NumberOfCoins = 0;
        //NumberOfPotions = 0;
    }
}