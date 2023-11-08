using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCoins : MonoBehaviour
{
    int coins = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            coins++;
            //CoinsUI.get().setCoints(coins);
        }
    }
}
