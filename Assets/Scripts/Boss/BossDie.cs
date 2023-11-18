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
    [SerializeField] private Canvas canvasHealth;

    [SerializeField] private GameObject _rainbowProjectile;

    // private readonly BossMovement _bossMovement;
    private Rigidbody2D rb;
    private AttackRainbow attackRainbow;
    private Color originalRainboColour;
    private Vector3 originalRainboLocalScaleVector;
    private float originalGrowingLocalScaleX;
    private float originalGrowingLocalScaleY;
    private float originalHiForce;
    private bool isHappend = false;

    private void Start()
    {
    
            bossHealth = GetComponentInParent<Health>();
            attackRainbow = new AttackRainbow(_rainbowProjectile, bossMovement);
            originalRainboColour = _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            originalRainboLocalScaleVector = _rainbowProjectile.transform.GetChild(0).localScale;
            originalGrowingLocalScaleX = attackRainbow.growingLocalScaleX;
            originalGrowingLocalScaleY = attackRainbow.growingLocalScaleY;
            originalHiForce = _rainbowProjectile.transform.GetChild(0).GetComponent<WaterProjectile>().HitForce;
        
    }

    private IEnumerator changeIntoOriginalRainbowmForm(float lengthAnimation)
    {
        yield return new WaitForSeconds(lengthAnimation);

        _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = originalRainboColour;
        _rainbowProjectile.transform.GetChild(0).localScale = originalRainboLocalScaleVector;
        attackRainbow.growingLocalScaleX = originalGrowingLocalScaleX;
        attackRainbow.growingLocalScaleY = originalGrowingLocalScaleY;
        _rainbowProjectile.transform.GetChild(0).GetComponent<WaterProjectile>().HitForce = originalHiForce;
    }

    private void Update()
    {
        if (bossHealth.IsDead()&& !isHappend)
        {
            createCustonRainbowAttack();
            bossMovement.canMove = false;
            ToggleBossState(true);
            StartCoroutine(attackRainbow.AppearRainbow(0f, 1.4f));
            StartCoroutine(waitDuration(1.4f));
            StartCoroutine(changeIntoOriginalRainbowmForm(3f));
            canvasHealth.transform.position += new Vector3(0, 0.9f, 0);
            GetComponentInParent<BoxCollider2D>().size += new Vector2(0, 0.12f);
            GetComponentInParent<BoxCollider2D>().offset += new Vector2(0, 0.06f);
            isHappend = true;
        }
    }

    private IEnumerator waitDuration(float lengthAnimation)
    {
        yield return new WaitForSeconds(lengthAnimation);
        bossMovement.canMove = true;
    }

    private void ToggleBossState(bool isGrown)
    {
        GrownBossVisuals.SetActive(isGrown);
        SmallBossVisuals.SetActive(isGrown == false);
    }

    private void createCustonRainbowAttack()
    {
        Color rainbowColor = _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        rainbowColor.r = 0;
        _rainbowProjectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = rainbowColor;
        _rainbowProjectile.transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0);
        attackRainbow.growingLocalScaleX = 0.009f;
        attackRainbow.growingLocalScaleY = 0.01f;
        _rainbowProjectile.transform.GetChild(0).GetComponent<WaterProjectile>().HitForce = 40;
    }
}