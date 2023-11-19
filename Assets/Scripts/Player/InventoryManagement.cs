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
        //_health = GetComponentInParent<Health>();
        NumberOfPotions = 0;
        NumberOfCoins = 0;
        IsSwordUpgraded = false;
    }
}