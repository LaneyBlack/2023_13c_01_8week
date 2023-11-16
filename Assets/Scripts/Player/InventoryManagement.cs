using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenoryManagment : MonoBehaviour
{
    public static bool IsSwordUpgraded = false; 
    public static int NumberOfCoins = 0;
    public static int NumberOfPotions = 0;
    public static List<int> KeysId = new List<int>();
    [SerializeField] private KeyCode _keyCode = KeyCode.P;
    private Health _health;

    private void Start()
    {
        _health = GetComponentInParent<Health>();
        IsSwordUpgraded = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(_keyCode))
        {
            if (NumberOfPotions > 0)
            {
                _health.RestoreHealth(1);
                NumberOfPotions--;
            }
        }
    }
}