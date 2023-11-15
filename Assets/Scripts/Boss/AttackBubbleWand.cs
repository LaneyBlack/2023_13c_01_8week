using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBubbleWand : MonoBehaviour
{
    private BossMovement bossMovement;
    private Transform attackPoint;
    private LayerMask playerLayers;
    public AttackBubbleWand(BossMovement bossMovement, Transform attackPoint, LayerMask playerLayers)
    {
        this.bossMovement = bossMovement;
        this.attackPoint = attackPoint;
        this.playerLayers = playerLayers;
    }
    public IEnumerator SmallAttackAnimationDuration()
    {
        yield return new WaitForSeconds(0.5f);
        bossMovement.canMove = true;
    }

    public void SmallAttackColliders()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1.4f, playerLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit and its currenntHealth =" + enemy.GetComponent<Health>().CurrentHealth);
            enemy.GetComponent<Health>().TakeDamage(1);
        }
    }
}
