using System.Collections.Generic;
using Unity.VisualScripting;
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

enum typeeq
{
    Sword,
    Hook
}

public class Inventory : MonoBehaviour
{
    [Header("Current item")]
    [SerializeField] private typeeq startItem;
    [HideInInspector] public int currentEquipped { get; private set; }

    [Header("Weapon setup")]
    [SerializeField] private KeyCode weaponKey;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private Sprite upgradedWeaponIcon;
    [SerializeField] private PlayerAttackSword weaponScript;
    private bool upgradeNoted = false;
    

    [Header("Hook setup")]
    [SerializeField] private KeyCode hookKey;
    [SerializeField] private Sprite HookIcon;
    [SerializeField] private HookGun hookScript;


    [Header("Potion setup")]
    [SerializeField] private KeyCode potionKey;
    [SerializeField] private Sprite potionIcon;
    //private HealthPotion potionScript;

    public List<Item> itemsData;

    private void Awake()
    {
        currentEquipped = (int)startItem;               
        itemsData = new List<Item>
        {
            new Item(weaponKey, weaponIcon, weaponScript),
            new Item(hookKey, HookIcon, hookScript),
            new Item(potionKey, potionIcon, null, 0)
        };

        foreach (Item item in itemsData)
            item.setEquipped(false);        //all is unequipped

        itemsData[currentEquipped].setEquipped(true);     
    }

    //private void Start()
    //{
    //    potionScript = GameObject.FindGameObjectWithTag("HealthPotion").GetComponent<HealthPotion>();
    //}

    private void Update()
    {
        if(!upgradeNoted && InvenoryManagment.IsSwordUpgraded)
        {
            upgradeNoted = true;
            itemsData[(int)typeeq.Sword].sprite = upgradedWeaponIcon;
        }

        int foundIndex = itemsData.FindIndex(item => Input.GetKeyDown(item.keycode));
        if (foundIndex != -1 && foundIndex < 2) 
        {
            itemsData[currentEquipped].setEquipped(false);
            currentEquipped = foundIndex;
            itemsData[currentEquipped].setEquipped(true);
        }

        if (foundIndex == 2)
            HealthPotion.usePotion();
    }
}
