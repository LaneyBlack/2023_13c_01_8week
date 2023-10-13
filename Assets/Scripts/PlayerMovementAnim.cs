using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnim : MonoBehaviour
{
    public Animator _Animator;

    // Update is called once per frame
    void Update()
    {
        _Animator.SetBool("Run", Input.GetAxis("Horizontal") != 0);
    }
}
