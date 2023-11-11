using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 currentCheckpoint;
    private Die deathsctipt;

    private void Awake()
    {
        deathsctipt = GetComponent<Die>();
    }

    private void Start()
    {
        currentCheckpoint = transform.position;
    }

    public void respawn()
    {
        transform.position = currentCheckpoint;
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
