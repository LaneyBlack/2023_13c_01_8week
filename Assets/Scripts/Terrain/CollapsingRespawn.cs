using System.Collections;
using UnityEngine;

public class CollapsingPlatformManager : MonoBehaviour
{
    public void RespawnPlatform(GameObject platform, Vector3 initialPosition, float delay)
    {
        StartCoroutine(RespawnCoroutine(platform, initialPosition, delay));
    }

    private IEnumerator RespawnCoroutine(GameObject platform, Vector3 initialPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        platform.transform.position = initialPosition;
        Rigidbody2D platformRigidbody = platform.GetComponent<Rigidbody2D>();
        if (platformRigidbody != null)
        {
            platformRigidbody.bodyType = RigidbodyType2D.Static;
        }
        Collapsing collapsingScript = platform.GetComponent<Collapsing>();
        if (collapsingScript != null)
        {
            collapsingScript.ResetPlatform();
        }
        platform.SetActive(true);
    }
}
