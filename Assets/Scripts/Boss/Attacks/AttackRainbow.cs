using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRainbow : MonoBehaviour
{
    private readonly GameObject _rainbowProjectile;
    private readonly BossMovement _bossMovement;
    public float growingLocalScaleX = 0.0055f;
    public float growingLocalScaleY = 0.0065f;

    public AttackRainbow(GameObject rainbowProjectile, BossMovement bossMovement)
    {
        _rainbowProjectile = rainbowProjectile;
        _bossMovement = bossMovement;
    }

    public IEnumerator AppearRainbow(float timeParticleAppear, float lengthOfAnimation)
    {
        yield return new WaitForSeconds(timeParticleAppear);
        _rainbowProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = _rainbowProjectile.transform.GetChild(0).localScale;
        Color originalColour = _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        while (time < lengthOfAnimation)
        {
            _rainbowProjectile.transform.GetChild(0).localScale += new Vector3(growingLocalScaleX,growingLocalScaleY, 0);
            Color currentColor = _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            currentColor.a -= 0.001f;
            currentColor.a = Mathf.Clamp01(currentColor.a);

            _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = currentColor;

            yield return null;
            time += Time.deltaTime;
        }

        _rainbowProjectile.transform.GetChild(0).localScale = originalScale;
        _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = originalColour;
        _rainbowProjectile.SetActive(false);
        _bossMovement.canMove = true;
        BossGrownAttack.IsGrownAttackFinished = true;
    }
}