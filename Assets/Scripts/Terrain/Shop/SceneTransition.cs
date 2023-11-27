using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    private enum GameScene
    {
        Tutorial,
        Shop,
        Level1,
        Level2,
        BossLevel
    }

    private int _currentLevel = 0;
    private const int _maxLevel = 4;
    private static SceneTransition _instance;

    private bool _isNearShop = false;
    private bool _isNearExit = false;

    private bool _firstTimeInShop = true;

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
            TransitionToScene(GameScene.Shop);
            if (_firstTimeInShop)
            {
                _currentLevel += 1;
                _firstTimeInShop = false;
            }
        }

        if (_isNearExit && Input.GetKeyDown(KeyCode.F) && SceneManager.GetActiveScene().name == GameScene.Shop.ToString())
        {
            IncrementLevel();
            TransitionToScene(GetNextLevelScene());
        }
    }

    private void IncrementLevel()
    {
        _currentLevel = (_currentLevel + 1) % (_maxLevel + 1);
    }

    private GameScene GetNextLevelScene()
    {
        return (GameScene)(_currentLevel % (_maxLevel + 1));
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject exitObject = GameObject.FindGameObjectWithTag("ExitShop");
        if (exitObject)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                player.transform.position = new Vector3(exitObject.transform.position.x, exitObject.transform.position.y - 1, exitObject.transform.position.z);

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
