using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 currentCheckpoint;
    [SerializeField] private Die deathsctipt;

    private void Start()
    {
        currentCheckpoint = transform.parent.position;
    }

    public void respawn()
    {
        transform.parent.position = currentCheckpoint;
        deathsctipt.handleRespawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform.position;
            collision.GetComponent<Collider2D>().enabled = false;
        }
    }
}
