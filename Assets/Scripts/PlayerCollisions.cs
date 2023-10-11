using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCollisions : MonoBehaviour
    {
        private BasicPlayerMovement _movement;
        private void Awake()
        {
            _movement = GetComponentInChildren<BasicPlayerMovement>();
        }

        
        private void OnCollisionEnter2D(Collision2D col)
        {
            _movement._isGrounded = true;
        }

      
        private void OnCollisionExit2D(Collision2D col)
        {
            _movement._isGrounded = false;
        }
    }
}