using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockedChestOpener : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField, Range(0f, 1f)]
    private float chanceDroppingPotion= 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float chanceDroppingDiamonds= 1f;
    
    [SerializeField] private GameObject diamondPrefab; 
    private Vector2 spawnOffsetDiamond = new Vector2(1f, 0f); 
    
    
    [SerializeField] private GameObject potionPrefab; 
    private Vector2 spawnOffsetPotion = new Vector2(1f, 0f); 
    
    
    public Animator animator;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InvenoryManagment.KeysId.Contains(id))
        {
            InvenoryManagment.KeysId.Remove(id);
            anim.SetTrigger("OpenChest");
            Invoke("SpawnDiamond", 1.45f); 
            Invoke("SpawnPotion", 1.45f); 
            Destroy(gameObject, 1.5f);
        }
    }
    private void SpawnDiamond()
    {
        spawnOffsetDiamond.x = Random.Range(-1f, 1f);
        if (Random.value <= chanceDroppingDiamonds)
        {
            Vector3 diamondPosition = transform.position + new Vector3(spawnOffsetDiamond.x, spawnOffsetDiamond.y, 0f);
            Instantiate(diamondPrefab, diamondPosition, Quaternion.identity);
        }
    }
    
    private void SpawnPotion()
    {
        spawnOffsetPotion.x = Random.Range(-1f, 1f);
        if (Random.value <= chanceDroppingPotion)
        {
            Vector3 potionPosition = transform.position + new Vector3(spawnOffsetPotion.x, spawnOffsetPotion.y, 0f);
            Instantiate(potionPrefab, potionPosition, Quaternion.identity);
        }
    }
}