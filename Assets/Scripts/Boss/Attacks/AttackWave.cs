using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWave : MonoBehaviour
{
    private readonly GameObject _waveProjectile;
    private readonly Transform _parentTransform;
    private readonly GameObject _player;
    private readonly BossMovement _bossMovement;

    public AttackWave(GameObject waveProjectile, Transform parentTransform, GameObject player, BossMovement bossMovement)
    {
        _waveProjectile = waveProjectile;
        _parentTransform = parentTransform;
        _player = player;
        _bossMovement = bossMovement;
    }

    public IEnumerator AppearWave(float timeParticleAppear, float lengthOfAnimation)
    {
        yield return new WaitForSeconds(timeParticleAppear);
        _waveProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = _waveProjectile.transform.localScale;
        bool isFlip = (_parentTransform.position.x - _player.transform.position.x) < 0;
        if (isFlip)
        {
            _waveProjectile.GetComponent<WaterProjectile>().changePosition(0, true);

            _waveProjectile.transform.localPosition = new Vector3(0.35f, 0.071f, 0); //prawo
        }
        else
        {
            _waveProjectile.GetComponent<WaterProjectile>().changePosition(0, false);

            _waveProjectile.transform.localPosition = new Vector3(-0.35f, 0.071f, 0);
        }

        while (time < lengthOfAnimation)
        {
            if (isFlip)
            {
                _waveProjectile.transform.localPosition += new Vector3(0.01f, 0, 0);
            }
            else
            {
                _waveProjectile.transform.localPosition += new Vector3(-0.01f, 0, 0);
            }
            Debug.Log("time: "+time);
            yield return null;
            time += Time.deltaTime;
        }

        _waveProjectile.transform.localScale = originalScale;
        _waveProjectile.SetActive(false);
        _bossMovement.canMove = true;
        Debug.Log("Last time: "+ time);
        BossSmallAttack.IsSmallAttackFinished = true;
    }
}
// public IEnumerator AppearBubble(float timeParticleAppear, float lengthOfAnimation)
// {
//     yield return new WaitForSeconds(timeParticleAppear);
//     _bubbleProjectile.SetActive(true);
//     float time = 0;
//     Vector3 originalScale = _bubbleProjectile.transform.localScale;
//     bool isFlip = (_parentTransform.position.x - _player.transform.position.x) < 0;
//     if (isFlip)
//     {
//         _bubbleProjectile.transform.localPosition = new Vector3(0.05f, -0.09f, 0); //prawo
//     }
//     else
//     {
//         _bubbleProjectile.transform.localPosition = new Vector3(-0.08f, -0.09f, 0);
//     }
//
//     while (time < lengthOfAnimation)
//     {
//         if (isFlip)
//         {
//             _bubbleProjectile.transform.localPosition += new Vector3(0.005f, 0, 0);
//         }
//         else
//         {
//             _bubbleProjectile.transform.localPosition += new Vector3(-0.005f, 0, 0);
//         }
//
//         yield return null;
//         time += Time.deltaTime;
//     }
//
//     _bubbleProjectile.transform.localScale = originalScale;
//     _bubbleProjectile.SetActive(false);
//     _bossMovement.canMove = true;
//     BossSmallAttack.IsAttackFinished = true;
// }
// }