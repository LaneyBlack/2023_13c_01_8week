using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBubble : MonoBehaviour
{
    private readonly GameObject _bubbleProjectile;
    private readonly Transform _parentTransform;
    private readonly GameObject _player;
    private readonly BossMovement _bossMovement;

    public AttackBubble(GameObject bubbleProjectile, Transform parentTransform, GameObject player,
        BossMovement bossMovement)
    {
        _bubbleProjectile = bubbleProjectile;
        _parentTransform = parentTransform;
        _player = player;
        _bossMovement = bossMovement;
    }

    public IEnumerator AppearBubble(float timeParticleAppear, float lengthOfAnimation)
    {
        yield return new WaitForSeconds(timeParticleAppear);
        _bubbleProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = _bubbleProjectile.transform.localScale;
        bool isFlip = (_parentTransform.position.x - _player.transform.position.x) < 0;
        if (isFlip)
        {
            _bubbleProjectile.transform.localPosition = new Vector3(0.05f, -0.09f, 0); //prawo
        }
        else
        {
            _bubbleProjectile.transform.localPosition = new Vector3(-0.08f, -0.09f, 0);
        }

        while (time < lengthOfAnimation)
        {
            if (isFlip)
            {
                _bubbleProjectile.transform.localPosition += new Vector3(0.005f, 0, 0);
            }
            else
            {
                _bubbleProjectile.transform.localPosition += new Vector3(-0.005f, 0, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        _bubbleProjectile.transform.localScale = originalScale;
        _bubbleProjectile.SetActive(false);
        _bossMovement.canMove = true;
        BossSmallAttack.IsSmallAttackFinished = true;
    }
}