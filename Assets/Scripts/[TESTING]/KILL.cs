using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KILL : MonoBehaviour
{
    public Health health;
    public KeyCode code;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(code))
        {
            health.TakeDamage(1);
        }
    }
}
