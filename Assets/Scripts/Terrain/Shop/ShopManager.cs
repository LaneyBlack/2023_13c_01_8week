using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _buyPotionButton;
    [SerializeField] private Button _buySwordButton;
    [SerializeField] private TextMeshProUGUI _playerMoneyText;

    private int _playerMoney = 100; // Placeholder
    
    //private bool _hasPotion = false;
    //private bool _hasSword = false;

    private void Start()
    {
        _buyPotionButton.onClick.AddListener(BuyPotion);
        _buySwordButton.onClick.AddListener(BuySword);

        //UpdateMoneyDisplay();
    }

    private void BuyPotion()
    {
        int potionPrice = 20;

        if (_playerMoney >= potionPrice)
        {
            _playerMoney -= potionPrice;
            Debug.Log("Bought a potion!");
            //UpdateMoneyDisplay();
        }
        else
        {
            Debug.Log("Not enough money to buy a potion.");
        }
    }

    private void BuySword()
    {
        int swordPrice = 50;

        if (_playerMoney >= swordPrice)
        {
            _playerMoney -= swordPrice;
            Debug.Log("Bought a sword!");
            //UpdateMoneyDisplay();
        }
        else
        {
            Debug.Log("Not enough money to buy a sword.");
        }
    }

    private void UpdateMoneyDisplay()
    {
        _playerMoneyText.text = "Money: " + _playerMoney;
    }
}