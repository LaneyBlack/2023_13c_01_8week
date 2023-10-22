using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthBarFull;
    [SerializeField] private Image healthBarCurrent;

    private void Start()
    {
        healthBarFull.fillAmount = playerHealth.CurrentHealth / 10f;
    }

    private void Update()
    {
        healthBarCurrent.fillAmount = playerHealth.CurrentHealth / 10f;
    }

}

