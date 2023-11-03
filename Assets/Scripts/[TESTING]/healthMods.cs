using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KILL : MonoBehaviour
{
    public Health health;
    public KeyCode codedmg;
    public KeyCode codeheal;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(codedmg))
        {
            health.TakeDamage(1);
        }

        if (Input.GetKeyDown(codeheal))
        {
            health.TakeDamage(-1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            health.KillForTesting();
    }
}
