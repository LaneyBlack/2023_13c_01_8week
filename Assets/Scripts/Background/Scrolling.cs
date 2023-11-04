using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [Range(-2,2)]
    public float scrollSpeed = 1.0f;
    [SerializeField]
    public float repositionX = -14.0f;
    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Update() {
        transform.Translate(Vector3.left * (scrollSpeed * Time.deltaTime));
        // Check if the cloud is out of view and move it back
        if (transform.position.x < repositionX) {
            RepositionCloud();
        }
    }

    private void RepositionCloud() {
        transform.position = _startPos;
    }
}
