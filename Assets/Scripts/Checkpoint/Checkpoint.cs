using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool manualCollisionSet = false;
    private BoxCollider2D boxCollider;

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
        var leftCorner = new Vector3(boxCollider.bounds.center.x - boxCollider.bounds.size.x / 2f, boxCollider.bounds.center.y);
        var rightCorner = new Vector3(boxCollider.bounds.center.x + boxCollider.bounds.size.x / 2f, boxCollider.bounds.center.y);

        float maxTraceLength = 20f;
        var left = Physics2D.Raycast(leftCorner, Vector3.up, maxTraceLength, LayerMask.GetMask("Ground"));
        var right = Physics2D.Raycast(rightCorner, Vector3.up, maxTraceLength, LayerMask.GetMask("Ground"));

        if (left.collider != null)
        {
            float fullSize = left.distance;

            Debug.DrawRay(leftCorner, Vector2.up * left.distance, Color.magenta, 120);

            if (right.collider != null)
            {
                Debug.DrawRay(rightCorner, Vector2.up * right.distance, Color.cyan, 120);
                fullSize = Mathf.Min(fullSize, right.distance);
            }

            fullSize += boxCollider.bounds.size.y / 2;
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
