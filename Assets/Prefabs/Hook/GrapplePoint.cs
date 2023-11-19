using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public float                    minSwingRadius = 3f;           //how close can player by to the grapple point in swing
    private float                   promptRadius;                  //distance when "can attach" color starts to change       
    private SpriteRenderer          spriteRenderer;
    [SerializeField] private float  attachError = 0.5f;            //how much further can player be from attachRadius and still attach
    [SerializeField] private float  attachRadius = 10f;            //how close player must be to the point to attach himself
    /*[SerializeField]*/ private Color  canAttachColor = Color.green;
    /*[SerializeField]*/ private Color  defaultColor = Color.red;

    float distance = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        promptRadius = attachRadius * 1.5f;
    }

    private void Update()
    {
        //var grapplingGun = GameObject.FindWithTag("GrapplingGun");

        //if (grapplingGun != null) 
        //{
        //    float distance = Vector2.Distance(grapplingGun.transform.position, transform.position);
        //    //Debug.Log("GrapplingGun found. Distance = " + distance);

        //    spriteRenderer.color = Color.Lerp(canAttachColor, defaultColor, (distance - attachRadius) / (promptRadius - attachRadius));
        //    //var hookgun = grapplingGun.GetComponent<HookGun>();
        //    //if (!hookgun.canHook)
        //    //{
        //    //    hookgun.canHook = (distance - attachError <= attachRadius);
        //    //    hookgun.grapplePoint = transform.position;
        //    //}
        //}

        //distance = Vector2.Distance(grapplingGun.transform.position, transform.position);
        spriteRenderer.color = Color.Lerp(canAttachColor, defaultColor, (distance - attachRadius) / (promptRadius - attachRadius));
    }

    public bool canAttach(Vector3 position)
    {
        distance = Vector2.Distance(position, transform.position);
        return Vector2.Distance(position, transform.position) <= (attachRadius + attachError);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attachRadius);
    }
}
