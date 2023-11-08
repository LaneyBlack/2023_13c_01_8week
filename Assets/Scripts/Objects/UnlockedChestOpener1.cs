using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnlockedChestOpener : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float chanceDroppingMonster = 1f;
    [SerializeField, Range(0, 4)] private int maxNumberOfSilverCoins = 3;
    [SerializeField, Range(0, 4)] private int maxNumberOfGoldCoins = 1;


    [SerializeField] private GameObject silverCoinPrefab;
    [SerializeField] private GameObject goldCoinPrefab;
    [SerializeField] private GameObject monsterPrefab;
    private Vector2 spawnOffsetMonster = new Vector2(1f, 0.1f);

    public Animator animator;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("OpenChest");
            Invoke("SpawnMonster", 0.95f);
            Invoke("SpawnRandomCoins", 0.95f);
            Destroy(gameObject, 1f);
        }
    }

    private void SpawnMonster()
    {
        spawnOffsetMonster.x = Random.Range(-1f, 1f);
        if (Random.value <= chanceDroppingMonster)
        {
            Vector3 monsterPosition = transform.position + new Vector3(spawnOffsetMonster.x, spawnOffsetMonster.y, 0f);
            GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
            monster.GetComponent<MeleeEnemy>().player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void SpawnRandomCoins()
    {
        var randomNumberOfSilverCoins = Random.Range(0, maxNumberOfSilverCoins);
        var randomNumberOfGoldCoins = Random.Range(0, maxNumberOfGoldCoins);
        var usedPositions = new List<Vector2>();
        for (var i = 0; i < randomNumberOfSilverCoins; i++)
        {
            Vector2 spawnPosition = GetUniquePosition(usedPositions);
            Instantiate(silverCoinPrefab, spawnPosition, Quaternion.identity);
            usedPositions.Add(spawnPosition);
        }

        for (var i = 0; i < randomNumberOfGoldCoins; i++)
        {
            Vector2 spawnPosition = GetUniquePosition(usedPositions);
            Instantiate(goldCoinPrefab, spawnPosition, Quaternion.identity);
            usedPositions.Add(spawnPosition);
        }
    }

    private Vector2 GetUniquePosition(ICollection<Vector2> usedPositions)
    {
        Vector2 spawnPosition;
        do
        {
            spawnPosition = (Vector2)transform.position + new Vector2(Random.Range(-1f, 1f), 0);
        } while (usedPositions.Contains(spawnPosition));

        return spawnPosition;
    }
}