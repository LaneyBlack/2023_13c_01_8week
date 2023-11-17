using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWaterProjectile : MonoBehaviour
{
    private readonly GameObject _waterProjectile;
    private readonly Transform _parentTransform;
    private readonly GameObject _player;
    private readonly BossMovement _bossMovement;
    public AttackWaterProjectile(GameObject waterProjectile, Transform parentTransform, GameObject player,
        BossMovement bossMovement)
    {
        _waterProjectile = waterProjectile;
        _parentTransform = parentTransform;
        _player = player;
        _bossMovement = bossMovement;
    }

    public IEnumerator AppearWaterProjectile(float timeParticleAppear, float lengthOfAnimation)
    {
        yield return new WaitForSeconds(timeParticleAppear);
        _waterProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = _waterProjectile.transform.localScale;

        while (time < lengthOfAnimation)
        {
            bool isFlip = (_parentTransform.position.x - _player.transform.position.x) < 0;
            _waterProjectile.transform.localScale += new Vector3(0.007f, 0, 0);
            _waterProjectile.GetComponent<SpriteRenderer>().flipX = isFlip;
            if (isFlip)
            {
                _waterProjectile.transform.localPosition = new Vector3(-0.07f, 0, 0); //prawo

                _waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(0.15f, true);
            }
            else
            {
                _waterProjectile.transform.localPosition = new Vector3(0.04f, 0, 0);
                _waterProjectile.GetComponentInChildren<WaterProjectile>().changePosition(-0.15f, false);
            }

            yield return null;
            time += Time.deltaTime;
        }

        _waterProjectile.transform.localScale = originalScale;
        _waterProjectile.SetActive(false);
        _bossMovement.canMove = true;
        BossGrownAttack.IsGrownAttackFinished = true;
    }

}
