using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] public int id = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (InvenoryManagment.KeysId.Contains(id))
            {
                Debug.LogError("THERE IS ALREADY THIS KEY ID! id -> " + id);
            }
            else
            {
                InvenoryManagment.KeysId.Add(id);
            }

            Destroy(gameObject);
        }
    }
}