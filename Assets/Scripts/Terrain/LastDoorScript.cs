using UnityEngine;
using UnityEngine.SceneManagement;

public class LastDoorScript : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

