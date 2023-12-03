using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public float                    minSwingRadius = 3f;           //how close can player by to the grapple point in swing
    private float                   promptRadius;                  //distance when "can attach" color starts to change       
    private Color                   canAttachColor = Color.green;
    private Color                   defaultColor = Color.red;
    private SpriteRenderer          spriteRenderer;
    public float attachRadius = 10f;           //how close player must be to the point to attach himself

    float distance = 0f;
    Animator animator;
    bool didset = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        promptRadius = attachRadius * 1.5f;
        didset = false;
        //transform.position -= new Vector3(0, -2f, 0);
        //spriteRenderer.color = 
    }

    private void Update()
    {
        //spriteRenderer.color = distance <= attachRadius ? canAttachColor : defaultColor;
        //spriteRenderer.color = Color.Lerp(canAttachColor, defaultColor, (distance - attachRadius) / (promptRadius - attachRadius));
        if (distance <= (attachRadius + .5f))
        {
            animator.SetBool("appear", true);
            //didset = true;
        }
        else
        {
            animator.SetBool("appear", false);
            //animator.Play("idle");
            //didset = false;
        }

    }

    public bool canAttach(Vector3 position)
    {
        distance = Vector2.Distance(position, transform.position);
        return distance <= (attachRadius + .5f);
    }

    private void OnDrawGizmos()
    {
        //attach radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attachRadius);

        //swing radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minSwingRadius);
    }
}
