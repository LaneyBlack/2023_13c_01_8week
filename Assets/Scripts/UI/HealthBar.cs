using System.Collections.Generic;
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
        FindPlayerHealth();
        if (playerHealth == null)
        {
            Debug.LogError("Player Health script not found!");
            return;
        }

        grid = GetComponent<GridLayoutGroup>();
        hearts = new List<GameObject>();

        previousHealth = playerHealth.CurrentHealth;

        var rectTransform = GetComponent<RectTransform>();
        int bckWidth = grid.padding.left * 2 + previousHealth * Mathf.RoundToInt(grid.cellSize.x) 
                        + Mathf.RoundToInt(grid.spacing.x) * (previousHealth - 1);

        rectTransform.sizeDelta = new Vector2(bckWidth, 130);

        for (int i = 0; i < previousHealth; i++)
        {
            var icon = Instantiate(healthIconPrefab, grid.transform);
            hearts.Add(icon);
        }
    }

    private void FindPlayerHealth()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }
    }

    private void Update()
    {
        if (playerHealth == null)
        {
            return;
        }

        int healthDiff = playerHealth.CurrentHealth - previousHealth;

        if (healthDiff > 0) // Player healing
        {
            for (int i = 0; i < playerHealth.CurrentHealth; i++)
            {
                hearts[i].GetComponent<Animator>().SetBool("stopbeat", false);
                hearts[i].GetComponent<Animator>().Play("HeartBeat", -1, 0);  //ensures that all anims play in sync
            }
        }

        if (healthDiff < 0) // Player taking damage
        {
            for (int i = previousHealth; i > playerHealth.CurrentHealth; i--)
                hearts[i - 1].GetComponent<Animator>().SetBool("stopbeat", true);
        }

        previousHealth = playerHealth.CurrentHealth;
    }
}
