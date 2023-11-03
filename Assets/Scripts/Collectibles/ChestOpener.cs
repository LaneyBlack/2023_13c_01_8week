using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestOpener : MonoBehaviour
{
    [SerializeField] private int id = 0;
    public Animator animator;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InvenoryManagment.KeysId.Contains(id))
        {
            InvenoryManagment.KeysId.Remove(id);
            anim.SetTrigger("OpenChest");
            Destroy(gameObject, 2f);
        }
    }
}