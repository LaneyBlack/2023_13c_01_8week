using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    // [SerializeField] private int damage;
    // [SerializeField] private float attackCooldown;
    // private float _cooldownTimer;
    // private Health _playerHealth;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject waterProjectile;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        _animator.SetTrigger("attack");
        
        StartCoroutine(SpawnProjectile());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject projectileInstance =
            Instantiate(waterProjectile, projectileSpawnPoint.position, Quaternion.identity);
        projectileInstance.transform.SetParent(this.transform);

        // Make the projectile follow the spawn point for 2 seconds.
        float time = 0;
        while (time < 1.1f)
        {
            bool isFlip=  ( transform.parent.position.x - player.transform.position.x) < 0;
            if (isFlip)
            {
                projectileInstance.GetComponent<SpriteRenderer>().flipX = isFlip;
                projectileSpawnPoint.localPosition = new Vector3(0.1f, projectileSpawnPoint.localPosition.y, projectileSpawnPoint.localPosition.z);
            }
            else
            {
                projectileInstance.GetComponent<SpriteRenderer>().flipX = isFlip;
                projectileSpawnPoint.localPosition = new Vector3(-0.1f, projectileSpawnPoint.localPosition.y, projectileSpawnPoint.localPosition.z);
            }
            // Ensure that the projectile has not been destroyed (e.g., by hitting something)
            if (projectileInstance != null)
            {
                projectileInstance.transform.position = projectileSpawnPoint.position;
            }
            else
            {
                break;
            }

            // Wait until next frame
            yield return null;
            time += Time.deltaTime;
        }

        // After 2 seconds destroy the projectile if it hasn't already been destroyed
        if (projectileInstance != null)
        {
            Destroy(projectileInstance);
        }
    }

}


// private IEnumerator SpawnProjectile()
// {
//     // Instantiate the projectile at the spawn point
//     GameObject projectileInstance = Instantiate(waterProjectile, projectileSpawnPoint.position, Quaternion.identity);
//     
//     // Determine if the mob is flipped based on the player's position
//     bool isFlipped = (transform.position.x - player.transform.position.x) < 0;
//     if (isFlipped)
//     {
//         // Flip the projectile's sprite
//         projectileInstance.GetComponent<SpriteRenderer>().flipX = true;
//
//         // Adjust the local position of the spawn point if needed
//         // For example, if you want to move it 2 units to the right when flipped
//         // This assumes your projectileSpawnPoint's local position is (0, 0) when not flipped
//         projectileSpawnPoint.localPosition = new Vector3(0.1f, projectileSpawnPoint.localPosition.y, projectileSpawnPoint.localPosition.z);
//     }
//     else
//     {
//         // If not flipped, make sure it's at the default local position
//         projectileSpawnPoint.localPosition = new Vector3(0, projectileSpawnPoint.localPosition.y, projectileSpawnPoint.localPosition.z);
//     }
//
//     // Now set the projectile instance at the spawn point's position
//     projectileInstance.transform.position = projectileSpawnPoint.position;
//
//     // Set the projectile as a child of the spawn point so it follows the mob (if that's what you want)
//     projectileInstance.transform.SetParent(projectileSpawnPoint);
//
//     // Destroy the projectile after 2 seconds
//     Destroy(projectileInstance, 2f);
//
//     yield return null;
// }
