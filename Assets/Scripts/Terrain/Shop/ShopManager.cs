using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _buyPotionButton;
    [SerializeField] private Button _buySwordButton;
    [SerializeField] private TextMeshProUGUI potionPriceText;
    [SerializeField] private TextMeshProUGUI swordPriceText;

    private int potionPrice = 20;
    private int swordPrice = 50;

    private void Start()
    {
        //InvenoryManagment.NumberOfCoins = 1000;
        _buyPotionButton.onClick.AddListener(BuyPotion);
        _buySwordButton.onClick.AddListener(BuySword);
        
        UpdatePotionPriceText();
        UpdateSwordPriceText();
        
        _shopUI.SetActive(false);
    }

    private void UpdatePotionPriceText()
    {
        if (potionPriceText != null)
        {
            potionPriceText.text = potionPrice.ToString();
        }
        else
        {
            Debug.LogError("PotionPrice TextMeshProUGUI component not assigned!");
        }
    }
    
    private void UpdateSwordPriceText()
    {
        if (swordPriceText != null)
        {
            swordPriceText.text = swordPrice.ToString();
        }
        else
        {
            Debug.LogError("PotionPrice TextMeshProUGUI component not assigned!");
        }
    }

    private void BuyPotion()
    {
        if (InvenoryManagment.NumberOfCoins >= potionPrice)
        {
            InvenoryManagment.NumberOfCoins -= potionPrice;
            InvenoryManagment.NumberOfPotions += 1;
        }
        else
        {
            Debug.Log("Not enough money to buy a potion.");
        }
    }

    private void BuySword()
    {
        if (InvenoryManagment.NumberOfCoins >= swordPrice)
        {
            InvenoryManagment.NumberOfCoins -= swordPrice;
            InvenoryManagment.IsSwordUpgraded = true;
        }
        else
        {
            Debug.Log("Not enough money to buy a sword.");
        }
    }
}