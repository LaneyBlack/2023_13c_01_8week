using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool manualCollisionSet = false;
     BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        if (manualCollisionSet) return;
        updateCollision();

    }

    void updateCollision()
    {
        //var halfSize = boxCollider.bounds.size * 0.5f;
        var origin = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y);
        //var origin = boxCollider.bounds.center;

        float maxTraceLength = 20f;
        var hit = Physics2D.Raycast(origin, Vector3.up, maxTraceLength, LayerMask.GetMask("Ground"));

        float scaleY = boxCollider.transform.localScale.y;
        if (hit.collider != null)
        {

            //Debug.Log("got a hit. Distance = " + hit.distance);
            //Debug.DrawRay(origin, Vector2.up * hit.distance, Color.magenta, 120);

            var fullSize = hit.distance + boxCollider.bounds.size.y / 2;
            //Debug.Log("fullSize = " + fullSize);
            updateBoxCollider(fullSize);
        }
        else
            updateBoxCollider(maxTraceLength);
    }

    private void updateBoxCollider(float newSize)
    {
        float scaleY = boxCollider.transform.localScale.y;

        newSize /= scaleY;
        boxCollider.offset = new Vector2(boxCollider.offset.x, newSize / 2 - boxCollider.size.y / 2 + boxCollider.offset.y);
        boxCollider.size = new Vector2(boxCollider.size.x, newSize);
    }
}
