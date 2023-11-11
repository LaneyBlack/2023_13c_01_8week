using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    private Health bossHealth;
    [SerializeField] public GameObject GrownBossVisuals;

    [SerializeField] public GameObject SmallBossVisuals;


    private Rigidbody2D rb;

    private void Start()
    {
        bossHealth = GetComponentInParent<Health>();
    }

    private void Update()
    {
        if (bossHealth.IsDead())
        {
            ToggleBossState(true);
            bossHealth.RestoreHealth(bossHealth.maxHealth);
        }
    }

    private void ToggleBossState(bool isGrown)
    {
        GrownBossVisuals.SetActive(isGrown);
        SmallBossVisuals.SetActive(isGrown == false);
    }
}