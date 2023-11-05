using System.Collections.Generic;
using UnityEngine;

public class CannonballTrap : MonoBehaviour
{
    public GameObject _cannonballPrefab;
    public int _initialPoolSize = 10;
    [SerializeField] public float _spawnRate = 1f;

    private List<GameObject> _cannonballs;
    private BoxCollider2D _trapArea;
    private bool _playerInArea = false;

    private void Awake()
    {
        _trapArea = GetComponent<BoxCollider2D>();

        _cannonballs = new List<GameObject>();
        for (int i = 0; i < _initialPoolSize; i++)
        {
            GameObject obj = Instantiate(_cannonballPrefab, this.transform);
            obj.SetActive(false);
            _cannonballs.Add(obj);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInArea = true;
            InvokeRepeating("SpawnCannonball", 0f, 1f/_spawnRate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInArea = false;
            CancelInvoke("SpawnCannonball");
        }
    }

    void SpawnCannonball()
    {
        if (!_playerInArea) return;

        Bounds bounds = _trapArea.bounds;

        float spawnX = Random.Range(bounds.min.x, bounds.max.x);
        float cameraTopY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float buffer = 2.0f;
        float spawnY = cameraTopY + buffer;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, transform.position.z);
        GameObject cannonball = GetCannonball();
        cannonball.transform.position = spawnPosition;
        cannonball.transform.SetParent(this.transform);
    }

    GameObject GetCannonball()
    {
        foreach (GameObject obj in _cannonballs)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(_cannonballPrefab, this.transform);
        _cannonballs.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
