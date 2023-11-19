using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        FindAndFollowPlayer();
    }

    void FindAndFollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && virtualCamera != null)
        {
            virtualCamera.Follow = player.transform;
        }
    }
}