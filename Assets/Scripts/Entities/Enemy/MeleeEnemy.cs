using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _damage;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
