using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScrollingAndRespawning : MonoBehaviour
{
    [Range(-2,2)]
    public float scrollSpeed = 1.0f;
    [SerializeField]
    private float _repositionX;
    [SerializeField]
    private float _respawnX;

    [SerializeField] private bool isXGlobal = true;
    [SerializeField] private List<Transform> gameObjectTransforms;

    private void Awake()
    {
        if (isXGlobal)
        {
            var position = transform.position;
            _repositionX += position.x;
            _respawnX += position.x;
        }

        foreach (Transform itemTransform in transform.GetComponentsInChildren<Transform>())
        {
            gameObjectTransforms.Add(itemTransform);
            // Respawn point is set to PointSetX and startY
        }
        
    }

    private void Update() {
        
        // Check if the cloud is out of view and move it back
        foreach (var itemTransform in gameObjectTransforms)
        {
            itemTransform.Translate(Vector3.left * (scrollSpeed * Time.deltaTime));
            if (itemTransform.position.x < _repositionX) {
                RepositionCloud(itemTransform);
            }
        }
        
    }

    private void RepositionCloud(Transform itemTransform) {
        itemTransform.position = new Vector3(_respawnX, itemTransform.position.y, itemTransform.position.z); // same y, z and different x
    }
}
