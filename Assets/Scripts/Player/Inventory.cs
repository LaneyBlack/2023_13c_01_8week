using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public KeyCode keycode;
    public Sprite sprite;
    public int count;

    public Item(KeyCode code, Sprite sprite, int count = -1)
    {
        this.keycode = code;
        this.sprite = sprite;
        this.count = count;
    }
}

public class Inventory : MonoBehaviour
{
    [Header("Weapon setup")]
    [SerializeField] private KeyCode weaponKey;
    [SerializeField] private Sprite weaponIcon;


    [Header("Hook setup")]
    [SerializeField] private KeyCode hookKey;
    [SerializeField] private Sprite HookIcon;


    [Header("Potion setup")]
    [SerializeField] private KeyCode potionKey;
    [SerializeField] private Sprite potionIcon;

    public List<Item> itemsData;

    private void Awake()
    {
        itemsData = new List<Item>
        {
            new Item(weaponKey, weaponIcon),
            new Item(hookKey, HookIcon),
            new Item(potionKey, potionIcon, 0)
        };
    }
}
