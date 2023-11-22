using UnityEngine;

public class InvisibleCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Respawn respawnScript = other.GetComponent<Respawn>();
            if (respawnScript != null)
            {
                respawnScript.SetCheckpoint(transform.position);
            }
        }
    }
}