using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBubbleWand : MonoBehaviour
{
    private readonly BossMovement _bossMovement;
    private readonly Transform _attackPoint;
    private readonly LayerMask _playerLayers;
    private readonly float  _attackRadius;
    private const float HitForce = 20;

    public AttackBubbleWand(BossMovement bossMovement, Transform attackPoint, LayerMask playerLayers, float attackRadius)
    {
        _bossMovement = bossMovement;
        _attackPoint = attackPoint;
        _playerLayers = playerLayers;
        _attackRadius = attackRadius;
    }
    public IEnumerator SmallAttackAnimationDuration(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        _bossMovement.canMove = true;
        BossSmallAttack.IsSmallAttackFinished = true;
    }

    public void SmallAttackColliders()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRadius, _playerLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit and its currenntHealth =" + enemy.GetComponent<Health>().CurrentHealth);
            enemy.GetComponent<Health>().TakeDamage(1);//_sprite = GetComponent<SpriteRenderer>();
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(HitForce * (enemy.GetComponent<SpriteRenderer>().flipX?2:-2), HitForce), ForceMode2D.Impulse); // Kicks player

        }
    }
}
