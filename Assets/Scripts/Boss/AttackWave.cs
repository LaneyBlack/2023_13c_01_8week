using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWave : MonoBehaviour
{
    private GameObject waveProjectile;
    private Transform parentTransform;
    private GameObject player;
    private BossMovement bossMovement;

    public AttackWave(GameObject waveProjectile, Transform parentTransform, GameObject player, BossMovement bossMovement)
    {
        this.waveProjectile = waveProjectile;
        this.parentTransform = parentTransform;
        this.player = player;
        this.bossMovement = bossMovement;
    }

    public IEnumerator AppearWave()
    {
        yield return new WaitForSeconds(1.3f);
        waveProjectile.SetActive(true);
        float time = 0;
        Vector3 originalScale = waveProjectile.transform.localScale;
        bool isFlip = (parentTransform.position.x - player.transform.position.x) < 0;
        if (isFlip)
        {
            waveProjectile.GetComponent<WaterProjectile>().changePosition(0, true);

            waveProjectile.transform.localPosition = new Vector3(0.35f, 0.071f, 0); //prawo
        }
        else
        {
            waveProjectile.GetComponent<WaterProjectile>().changePosition(0, false);

            waveProjectile.transform.localPosition = new Vector3(-0.35f, 0.071f, 0);
        }

        while (time < 2f)
        {
            if (isFlip)
            {
                waveProjectile.transform.localPosition += new Vector3(0.01f, 0, 0);
            }
            else
            {
                waveProjectile.transform.localPosition += new Vector3(-0.01f, 0, 0);
            }

            yield return null;
            time += Time.deltaTime;
        }

        waveProjectile.transform.localScale = originalScale;
        waveProjectile.SetActive(false);
        bossMovement.canMove = true;
    }
}
