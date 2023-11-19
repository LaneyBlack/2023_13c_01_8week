using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _buyPotionButton;
    [SerializeField] private Button _buySwordButton;

    private void Start()
    {
        InvenoryManagment.NumberOfCoins = 1000;
        _buyPotionButton.onClick.AddListener(BuyPotion);
        _buySwordButton.onClick.AddListener(BuySword);
    }

    private void BuyPotion()
    {
        int potionPrice = 20;

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
        int swordPrice = 50;

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