using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    private enum GameScene
    {
        Shop,
        Tutorial,
        Level1,
    }

    private int _currentLevel = 1;
    private const int _maxLevel = 2;
    private string _previousSceneName;
    private bool _isNearShop = false;
    private bool _isNearExit = false;
    private static SceneTransition _instance;
    private string _currentExitTag;
    private Vector2 _lastPlayerPosition;

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
            _previousSceneName = SceneManager.GetActiveScene().name;
            _lastPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            TransitionToScene(GameScene.Shop);
        }

        if (_isNearExit && Input.GetKeyDown(KeyCode.F) && SceneManager.GetActiveScene().name == GameScene.Shop.ToString())
        {
            GoToNextLevel();
        }
    }

    private void TransitionToScene(GameScene scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(GameScene scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void GoToNextLevel()
    {
        _currentLevel++;
        if (_currentLevel > _maxLevel) _currentLevel = 1;
        TransitionToScene((GameScene)_currentLevel);
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
                if (_currentExitTag == "EnterShop")
                {
                    player.transform.position = _lastPlayerPosition;
                }
                else
                {
                    float offsetY = 1f;
                    player.transform.position = new Vector2(exitObject.transform.position.x, exitObject.transform.position.y - offsetY);
                }
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
