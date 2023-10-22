using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{
    public float pushForce = 5f;
    public LayerMask boxLayer;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.1f)
        {
            Push(Vector2.right);
        }
        else if (horizontalInput < -0.1f)
        {
            Push(Vector2.left);
        }
    }

    void Push(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, boxLayer);

        List<Rigidbody2D> boxes = new List<Rigidbody2D>();
        int safeguard = 0;

        while (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            if (++safeguard > 100)
            {
                break;
            }

            Rigidbody2D boxRb = hit.collider.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                boxes.Add(boxRb);
            }
            
            Vector2 nextRaycastOrigin = (Vector2)hit.collider.transform.position + direction * 1.1f;
            hit = Physics2D.Raycast(nextRaycastOrigin, direction, 1f, boxLayer);
        }
        
        float forceToApply = pushForce / Mathf.Max(1, boxes.Count);

        foreach (Rigidbody2D boxRb in boxes)
        {
            boxRb.AddForce(direction * forceToApply, ForceMode2D.Impulse);
        }
    }
}