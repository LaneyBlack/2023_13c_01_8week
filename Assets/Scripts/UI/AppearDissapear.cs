using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AppearDissapear : MonoBehaviour
{
    [SerializeField] private GameObject appearingObject;
    [SerializeField] private float timeToAppear = 10;
    private float timer;

    private void Start()
    {
        timer = timeToAppear;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            appearingObject.SetActive(!appearingObject.activeInHierarchy); // switch
            timer += timeToAppear;
        }
        timer -= Time.deltaTime;
    }
}
