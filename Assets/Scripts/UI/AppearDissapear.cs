using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDissapear : MonoBehaviour
{
    [SerializeField] private GameObject gameObject;
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
            gameObject.SetActive(!gameObject.activeInHierarchy); // switch
            timer += timeToAppear;
        }
        timer -= Time.deltaTime;
    }
}
