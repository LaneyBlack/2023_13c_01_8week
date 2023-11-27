using System.Collections;
using UnityEngine;

public class StickToMovingPlatform : MonoBehaviour
{
    private GameObject player;
    private Vector3 lastPlatformPosition;

    private void Start()
    {
        lastPlatformPosition = transform.position;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 deltaPosition = transform.position - lastPlatformPosition;
            player.transform.position += deltaPosition;
        }
        lastPlatformPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }
}