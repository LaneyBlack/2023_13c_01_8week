using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string _shopSceneName = "Shop";
    [SerializeField] private string _levelSceneName = "ShopTesting";

    private bool _isNearShop = false;
    private bool _isNearExit = false;
    private static SceneTransition _instance;
    private string _currentExitTag;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
        if (_isNearShop && Input.GetKeyDown(KeyCode.F))
        {
            _currentExitTag = "ExitShop";
            TransitionToScene(_shopSceneName);
        }

        if (_isNearExit && Input.GetKeyDown(KeyCode.F))
        {
            _currentExitTag = "EnterShop";
            TransitionToScene(_levelSceneName);
        }
    }

    private void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(_currentExitTag))
            return;

        GameObject exitObject = GameObject.FindGameObjectWithTag(_currentExitTag);
        if (exitObject)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                float offsetY = 1f;
                player.transform.position = new Vector2(exitObject.transform.position.x, exitObject.transform.position.y - offsetY);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnterShop"))
            _isNearShop = true;

        if (collision.CompareTag("ExitShop"))
            _isNearExit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnterShop"))
            _isNearShop = false;

        if (collision.CompareTag("ExitShop"))
            _isNearExit = false;
    }
}
