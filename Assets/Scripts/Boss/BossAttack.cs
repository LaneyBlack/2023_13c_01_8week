using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] public GameObject SmallBossVisuals;
    [SerializeField] private Animator _SmallBossAnimator;
    [SerializeField] private Animator _GrownBossAnimator;
    
    [SerializeField] private GameObject waterProjectile;
    [SerializeField] private Transform projectileSpawnPoint;
    private GameObject player; //
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f; // Cooldown between attacks

    private float
        _cooldownTimer = Mathf.Infinity; // Set the cooldown timer to a high number so the boss can attack immediately

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.parent.position, player.transform.position);
        if (distanceToPlayer <= attackRange && _cooldownTimer >= attackCooldown)
        {
            Attack();
            _cooldownTimer = 0f; // Reset the cooldown timer
        }
    }

    void Attack()
    {
        if (SmallBossVisuals.activeSelf)
        {
            _SmallBossAnimator.SetTrigger("Attack");
        }
        else
        {
            _GrownBossAnimator.SetTrigger("Attack");
        }

        StartCoroutine(SpawnProjectile());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject projectileInstance =
            Instantiate(waterProjectile, projectileSpawnPoint.position, Quaternion.identity);
        projectileInstance.transform.SetParent(this.transform);
        WaterProjectile waterProjectileScript = projectileInstance.GetComponent<WaterProjectile>();
        if (waterProjectileScript != null)
        {
            waterProjectileScript.Initialize(this.transform.parent.gameObject); // this.gameObject refers to the enemy
        }

        float time = 0;
        while (time < 1.1f)
        {
            bool isFlip = (transform.parent.position.x - player.transform.position.x) < 0;
            if (isFlip)
            {
                projectileInstance.GetComponent<SpriteRenderer>().flipX = isFlip;
                projectileSpawnPoint.localPosition = new Vector3(0.1f, projectileSpawnPoint.localPosition.y,
                    projectileSpawnPoint.localPosition.z);
            }
            else
            {
                projectileInstance.GetComponent<SpriteRenderer>().flipX = isFlip;
                projectileSpawnPoint.localPosition = new Vector3(-0.1f, projectileSpawnPoint.localPosition.y,
                    projectileSpawnPoint.localPosition.z);
            }

            if (projectileInstance != null)
            {
                projectileInstance.transform.position = projectileSpawnPoint.position;
            }
            else
            {
                break;
            }

            yield return null;
            time += Time.deltaTime;
        }

        if (projectileInstance != null)
        {
            Destroy(projectileInstance);
        }
    }
}