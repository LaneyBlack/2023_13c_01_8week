using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    private float                   promptRadius;           //distance when "can attach" color starts to change       
    private SpriteRenderer          spriteRenderer;
    [SerializeField] private float  attachRadius = 10f;     //how close player must be to the point to attach himself
    /*[SerializeField]*/ private Color  canAttachColor = Color.green;
    /*[SerializeField]*/ private Color  defaultColor = Color.red;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        promptRadius = attachRadius * 1.5f;
    }

    private void Update()
    {
        var player = GameObject.FindWithTag("Player");

        if (player != null) 
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);
            //Debug.Log("player found. Distance = " + distance);

            spriteRenderer.color = Color.Lerp(canAttachColor, defaultColor, (distance - attachRadius) / (promptRadius - attachRadius));
            player.GetComponent<PlayerHook>().canHook = (distance <= attachRadius);
        }
    }
}
