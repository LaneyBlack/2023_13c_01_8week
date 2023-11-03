using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnlockedChestOpener : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float chanceDroppingMonster= 1f;
    
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
        if (collision.CompareTag("Player") )
        {
            anim.SetTrigger("OpenChest");
            Invoke("SpawnMonster", 0.95f); 
            // Invoke("SpawnPotion", 1.45f); 
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
            MeleeEnemy meleeEnemyScript = monster.GetComponent<MeleeEnemy>();
            if (meleeEnemyScript != null)
            {
                meleeEnemyScript.player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    }
    

}