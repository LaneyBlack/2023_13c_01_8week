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
    [SerializeField] private float  attachError = 0.5f;            //how much further can player be from attachRadius and still attach
    [SerializeField] private float  attachRadius = 10f;            //how close player must be to the point to attach himself

    float distance = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        promptRadius = attachRadius * 1.5f;
    }

    private void Update()
    {
        spriteRenderer.color = Color.Lerp(canAttachColor, defaultColor, (distance - attachRadius) / (promptRadius - attachRadius));
    }

    public bool canAttach(Vector3 position)
    {
        distance = Vector2.Distance(position, transform.position);
        return Vector2.Distance(position, transform.position) <= (attachRadius + attachError);
    }

    private void OnDrawGizmosSelected()
    {
        //attach radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attachRadius);

        //swing radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minSwingRadius);
    }
}
