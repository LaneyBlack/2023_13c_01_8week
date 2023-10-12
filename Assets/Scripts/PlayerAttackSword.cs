using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSword : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    [SerializeField] private KeyCode _keyCode;
    void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            animator.SetTrigger("AttackSword");

        }
    }

}
