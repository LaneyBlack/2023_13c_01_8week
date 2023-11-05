using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DooorOpener : MonoBehaviour
{
    [SerializeField] private int id = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InvenoryManagment.KeysId.Contains(id))
        {
            InvenoryManagment.KeysId.Remove(id);
            Destroy(gameObject, 0.5f);
        }
    }
}