using System.Collections;
using UnityEngine;

public class StickToMovingPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            StartCoroutine(SetParentNextFrame(other.transform, transform));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            StartCoroutine(SetParentNextFrame(other.transform, null));
        }
    }

    private IEnumerator SetParentNextFrame(Transform child, Transform newParent)
    {
        yield return null;
        child.SetParent(newParent);
    }
}