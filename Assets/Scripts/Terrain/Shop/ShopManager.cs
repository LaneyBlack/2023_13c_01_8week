using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Button _buyPotionButton;
    [SerializeField] private Button _buySwordButton;

    private int _playerMoney = 100; // Placeholder
    //private bool _hasPotion = false;
    //private bool _hasSword = false;

    private void Start()
    {
        _buyPotionButton.onClick.AddListener(BuyPotion);
        _buySwordButton.onClick.AddListener(BuySword);
    }

    private void BuyPotion()
    {
        int potionPrice = 20;

        if (_playerMoney >= potionPrice)
        {
            _playerMoney -= potionPrice;
            //_hasPotion = true;

            Debug.Log("Bought a potion!");
            // TODO: Add the potion to the player's inventory.
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
            //_hasSword = true;

            Debug.Log("Bought a sword!");
            // TODO: Equip the sword or add it to the player's inventory.
        }
        else
        {
            Debug.Log("Not enough money to buy a sword.");
        }
    }
}