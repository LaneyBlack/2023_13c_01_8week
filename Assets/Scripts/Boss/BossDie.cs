using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    private Health bossHealth;
    [SerializeField] public GameObject GrownBossVisuals;

    [SerializeField] public GameObject SmallBossVisuals;
    [SerializeField] private BossMovement bossMovement;


    private Rigidbody2D rb;

    private void Start()
    {
        bossHealth = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if (bossHealth.IsDead())
        {
            bossMovement.canMove = false;
            ToggleBossState(true);
            StartCoroutine(waitDuration());
        }
    }
    private IEnumerator waitDuration()
    {
        yield return new WaitForSeconds(0.8f);
        bossMovement.canMove = true;
    }
    private void ToggleBossState(bool isGrown)
    {
        GrownBossVisuals.SetActive(isGrown);
        SmallBossVisuals.SetActive(isGrown == false);
    }
    
   
}