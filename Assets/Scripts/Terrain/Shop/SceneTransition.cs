using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private bool isNearShop = false;
    private bool isNearExit = false;
    
    public string shopSceneName = "Shop";
    public string levelSceneName = "ShopTesting";
    
    private static SceneTransition instance;
    private string currentExitTag; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (isNearShop && Input.GetKeyDown(KeyCode.F))
        {
            currentExitTag = "ExitShop";
            TransitionToScene(shopSceneName);
        }

        if (isNearExit && Input.GetKeyDown(KeyCode.F))
        {
            currentExitTag = "EnterShop";
            TransitionToScene(levelSceneName);
        }
    }

    private void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(currentExitTag))
            return;

        GameObject exitObject = GameObject.FindGameObjectWithTag(currentExitTag);
        if (exitObject != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = exitObject.transform.position; 
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnterShop"))
            isNearShop = true;
        
        if (collision.CompareTag("ExitShop"))
            isNearExit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnterShop"))
            isNearShop = false;
        
        if (collision.CompareTag("ExitShop"))
            isNearExit = false;
    }
}
