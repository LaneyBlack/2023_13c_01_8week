using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCollisions : MonoBehaviour
    {
        private BasicPlayerMovement _movement;
        private Animator _animator;
        private void Awake()
        {
            _movement = GetComponentInChildren<BasicPlayerMovement>();
            _animator = GetComponentInChildren<Animator>();
        }

        
        private void OnCollisionEnter2D(Collision2D col)
        {
            _movement._isGrounded = true;
            _animator.SetBool("Grounded", true);
        }

      
        private void OnCollisionExit2D(Collision2D col)
        {
            _movement._isGrounded = false;
            _animator.SetBool("Grounded", false);

        }
    }
}


// if (Input.GetAxis("Horizontal") < 0)
// {
//     transform.localScale = new Vector3(-5,5,0);
// }