using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthIconImage;
    [SerializeField] private GameObject healthIconPrefab;

    private GridLayoutGroup grid;
    private int previousHealth;
    private List<GameObject> hearts;

    private void Start()
    {
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
        int index;

        if (healthdiff > 0) 
        {
            index = Mathf.Clamp(playerHealth.CurrentHealth, 0, playerHealth.CurrentHealth - 1);
            hearts[index].GetComponent<Animator>().SetBool("shouldbeat", true);
        }

        if(healthdiff < 0)
        {
            index = Mathf.Clamp(previousHealth, 0, previousHealth - 1);
            hearts[index].GetComponent<Animator>().SetBool("shouldbeat", false);
        }

        previousHealth = playerHealth.CurrentHealth;
    }
}

