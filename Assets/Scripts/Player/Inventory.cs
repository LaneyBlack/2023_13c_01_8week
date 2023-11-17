using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public KeyCode keycode;
    public Sprite sprite;
    private Equippable script;
    public int count;
    public bool isEquipped;

    public Item(KeyCode code, Sprite sprite, Equippable script = null, int count = -1)
    {
        this.keycode = code;
        this.sprite = sprite;
        this.count = count;
        this.script = script;
    }

    public void setEquipped(bool isEquipped)
    {
        if (script != null & count == -1)
            script.isEquipped = isEquipped;

        //bad idea:
        //if (script != null & count == -1)
        //    script.enabled = isEquipped;
    }

    public bool isAvailable()   //rewrite
    {
        if (script != null & count == -1)
            return script.isEquipped;

        return false;
    }
}

public class Inventory : MonoBehaviour
{
    [Header("Weapon setup")]
    [SerializeField] private KeyCode weaponKey;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private PlayerAttackSword weaponScript;
    

    [Header("Hook setup")]
    [SerializeField] private KeyCode hookKey;
    [SerializeField] private Sprite HookIcon;
    [SerializeField] private HookGun hookScript;


    [Header("Potion setup")]
    [SerializeField] private KeyCode potionKey;
    [SerializeField] private Sprite potionIcon;
    [SerializeField] private HealthPotion potionScript;

    [HideInInspector] public int currentEquipped { get; private set; }
    public List<Item> itemsData;

    private void Awake()
    {
        currentEquipped = 0;
        itemsData = new List<Item>
        {
            new Item(weaponKey, weaponIcon, weaponScript),
            new Item(hookKey, HookIcon, hookScript),
            new Item(potionKey, potionIcon, null, 0)
        };

        foreach (Item item in itemsData)
            item.setEquipped(false);        //all is unequipped

        itemsData[0].setEquipped(true);     //set sword to be equipped by default
    }


    private void Update()
    {
        int foundIndex = itemsData.FindIndex(item => Input.GetKeyDown(item.keycode));
        if (foundIndex != -1 && foundIndex < 2) 
        {
            itemsData[currentEquipped].setEquipped(false);
            currentEquipped = foundIndex;
            itemsData[currentEquipped].setEquipped(true);
        }


    }
}
