using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{
    public float pushForce = 5f;
    public LayerMask boxLayer;
    void Update()
    {

        
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.1f) // Pushing to the right
        {
            Push(Vector2.right);
        }
        else if (horizontalInput < -0.1f) // Pushing to the left
        {
            Push(Vector2.left);
        }

    }

    void Push(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, boxLayer);

        List<Rigidbody2D> boxes = new List<Rigidbody2D>();

        while (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            Rigidbody2D boxRb = hit.collider.GetComponent<Rigidbody2D>();
            boxes.Add(boxRb);

            hit = Physics2D.Raycast(hit.collider.transform.position, direction, 1f, boxLayer);
        }
        
        float forceToApply = pushForce / Mathf.Max(1, boxes.Count); // Ensure we don't divide by zero

        foreach (Rigidbody2D boxRb in boxes)
        {
            boxRb.AddForce(direction * forceToApply, ForceMode2D.Impulse);
        }
    }
    
}