using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
     private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxFollowDistance;
    [SerializeField] private float minFollowDistance;
 

    [SerializeField] public GameObject SmallBossVisuals;

    [SerializeField] private Animator _SmallBossAnimator;
    [SerializeField] private SpriteRenderer _SmallBossSprite;
    [SerializeField] private Animator _GrownBossAnimator;
    [SerializeField] private SpriteRenderer _GrownBossSprite;
    
    private Rigidbody2D _rigidbody;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (SmallBossVisuals.activeSelf )
        {
            _SmallBossAnimator.SetBool(IsMoving, IsInSight());
            _SmallBossSprite.flipX = ( transform.parent.position.x - player.transform.position.x) < 0;

        }
        else
        {
            _GrownBossAnimator.SetBool(IsMoving, IsInSight());
            _GrownBossSprite.flipX = ( transform.parent.position.x - player.transform.position.x) < 0;
        }
       }

    private void FixedUpdate()
    {
        if (SmallBossVisuals.activeSelf )
        {
            if (_SmallBossAnimator.GetBool(IsMoving))
            {
                var direction = player.transform.position - transform.parent.position; 
                _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            }
        }
        else
        {
            if (_GrownBossAnimator.GetBool(IsMoving))
            {
                var direction = player.transform.position - transform.parent.position; 
                _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            }
        }
        
    
    }


    private bool IsInSight()
    {
        var distance = Vector2.Distance( transform.parent.position, player.transform.position);
        var isInSight = distance < maxFollowDistance && distance > minFollowDistance;
        return isInSight;
    }
}