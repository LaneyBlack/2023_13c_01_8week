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


// if (Input.GetAxis("Horizontal") < 0)
// {
//     transform.localScale = new Vector3(-5,5,0);
// }