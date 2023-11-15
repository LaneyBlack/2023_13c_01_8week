using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBubble : MonoBehaviour
{
    private GameObject bubbleProjectile;
    private Transform parentTransform;
    private GameObject player;
    private BossMovement bossMovement;

    public AttackBubble(GameObject bubbleProjectile, Transform parentTransform, GameObject player, BossMovement bossMovement)
    {
        this.bubbleProjectile = bubbleProjectile;
        this.parentTransform = parentTransform;
        this.player = player;
        this.bossMovement = bossMovement;
    }

    public IEnumerator AppearBubble()
    {
        yield return new WaitForSeconds(0.3f);
        bubbleProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = bubbleProjectile.transform.localScale;
        bool isFlip = (parentTransform.position.x - player.transform.position.x) < 0;
        if (isFlip)
        {
            bubbleProjectile.transform.localPosition = new Vector3(0.05f, -0.09f, 0); //prawo
        }
        else
        {
            bubbleProjectile.transform.localPosition = new Vector3(-0.08f, -0.09f, 0);
        }

        while (time < 1f)
        {
            if (isFlip)
            {
                bubbleProjectile.transform.localPosition += new Vector3(0.005f, 0, 0);
            }
            else
            {
                bubbleProjectile.transform.localPosition += new Vector3(-0.005f, 0, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        bubbleProjectile.transform.localScale = originalScale;
        bubbleProjectile.SetActive(false);
        bossMovement.canMove = true;
    }
}
