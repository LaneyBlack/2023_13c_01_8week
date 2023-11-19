using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] private Image healthIconImage;
    [SerializeField] private GameObject healthIconPrefab;

    private GridLayoutGroup grid;
    private int previousHealth;
    private List<GameObject> hearts;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        grid = GetComponent<GridLayoutGroup>();
        hearts = new List<GameObject> ();

        previousHealth = playerHealth.CurrentHealth;

        var transform = GetComponent<RectTransform>();
        int bckWidth =  grid.padding.left * 2 + previousHealth * Mathf.RoundToInt(grid.cellSize.x) 
                        + Mathf.RoundToInt(grid.spacing.x) * (previousHealth - 1);

        transform.sizeDelta = new Vector2(bckWidth, 130);

        for (int i = 0; i < previousHealth; i++)
        {
            var icon = Instantiate(healthIconPrefab, grid.transform);
            hearts.Add(icon);
        }
    }

    private void Update()
    {
        int healthdiff = playerHealth.CurrentHealth - previousHealth;

        if (healthdiff > 0) //player healing
        {
            for (int i = 0; i < playerHealth.CurrentHealth; i++)
            {
                hearts[i].GetComponent<Animator>().SetBool("stopbeat", false);
                hearts[i].GetComponent<Animator>().Play("HeartBeat", -1, 0);     //ensures that all anims play in sync
            }
        }

        if (healthdiff < 0) //player taking damage
        {
            for (int i = previousHealth; i > playerHealth.CurrentHealth; i--)
                hearts[i - 1].GetComponent<Animator>().SetBool("stopbeat", true);
        }

        previousHealth = playerHealth.CurrentHealth;
    }
}

