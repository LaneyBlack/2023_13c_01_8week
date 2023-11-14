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

    private BoxCollider2D boxCollider;

    // private bool isGrounded = true; 


    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    public bool canMove = true;

    [Header("Jump")] [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask jumpLayer;
    [SerializeField] private List<LayerMask> jumpLayers = new List<LayerMask>();
    private float groundRayLength = .1f;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();

        boxCollider = GetComponentInParent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (SmallBossVisuals.activeSelf)
        {
            _SmallBossAnimator.SetBool(IsMoving, IsInSight());
            _SmallBossSprite.flipX = (transform.parent.position.x - player.transform.position.x) < 0;
        }
        else
        {
            _GrownBossAnimator.SetBool(IsMoving, IsInSight());
            _GrownBossSprite.flipX = (transform.parent.position.x - player.transform.position.x) < 0;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        var direction = player.transform.position - transform.parent.position;

        if (SmallBossVisuals.activeSelf)
        {
            if (_SmallBossAnimator.GetBool(IsMoving))
            {
                _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            }
        }
        else
        {
            if (_GrownBossAnimator.GetBool(IsMoving))
            {
                _rigidbody.velocity = new Vector2(movementSpeed * Math.Sign(direction.x), _rigidbody.velocity.y);
            }
        }

        if (ShouldJump())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsInSight()
    {
        var distance = Vector2.Distance(transform.parent.position, player.transform.position);
        var isInSight = distance < maxFollowDistance && distance > minFollowDistance;
        return isInSight;
    }

    private bool ShouldJump()
    {
        if (!isGrounded())
        {
            return false;
        }

        var direction = player.transform.position - transform.parent.position;
        float directionSign = Mathf.Sign(direction.x);

        float rayStartOffset = 0.1f; 
        Vector2 origin = new Vector2(
            transform.position.x + (boxCollider.bounds.extents.x + rayStartOffset) * directionSign,
            boxCollider.bounds.min.y 
        );

        Vector2 diagonalDirection = new Vector2(directionSign, 2f); 
        float detectionDistance = 2f; 
        int bossLayer = gameObject.layer;
        Physics2D.IgnoreLayerCollision(bossLayer, bossLayer, true);

        RaycastHit2D hit = Physics2D.Raycast(origin, diagonalDirection, detectionDistance);

        Debug.DrawRay(origin, diagonalDirection * detectionDistance, Color.red);

        // If an elevation is detected in the path, jump
        if (hit.collider != null && Mathf.Abs(hit.point.y - boxCollider.bounds.min.y) > 0.1f)
        {
            return hit.point.y > boxCollider.bounds.min.y;
        }

        return false;
    }



    private bool isGrounded()
    {
        var collided = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            groundRayLength); //<- change to raycast??
        foreach (var col in collided)
        {
            if (col.collider == null) continue;
            if (jumpLayers.Contains(LayerMask.GetMask(LayerMask.LayerToName(col.collider.gameObject.layer))))
            {
                return true;
            }
        }

        return false;
    }
}