using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [Range(-2,2)]
    public float scrollSpeed = 1.0f;
    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Update() {
        transform.Translate(Vector3.left * (scrollSpeed * Time.deltaTime));
        // Check if the cloud is out of view and move it back to the top
        if (transform.position.x < -14.0f) {
            RepositionCloud();
        }
    }

    private void RepositionCloud() { // Set a random Y position above the screen
        transform.position = _startPos;
    }
}
