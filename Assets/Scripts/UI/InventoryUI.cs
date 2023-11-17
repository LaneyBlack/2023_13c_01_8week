using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory _inventory;
    private GridLayoutGroup _gridLayout;
    private List<Image> _slotImages;

    private void Start()
    {
        FindPlayerInventory();
        if (_inventory == null)
        {
            Debug.LogError("Inventory script not found on player!");
            return;
        }

        _gridLayout = GetComponent<GridLayoutGroup>();
        _slotImages = new List<Image>();

        var images = _gridLayout.GetComponentsInChildren<Image>();
        var texts = _gridLayout.GetComponentsInChildren<Text>();

        for (int i = 0, itemid = 0; i < images.Length; i++)
        {
            var x = images[i];

            if (x.gameObject.CompareTag("UI_Item"))
            {
                x.sprite = _inventory.itemsData[itemid++].sprite;
                _slotImages.Add(images[i]);
            }
        }

        string pattern = "Keypad|Alpha";
        for (int i = 0, itemid = 0; i < texts.Length; i++)
        {
            var x = texts[i];

            if (x.gameObject.CompareTag("UI_Key"))
                x.text = Regex.Replace(_inventory.itemsData[itemid++].keycode.ToString(), pattern, "");
        }
    }

    private void FindPlayerInventory()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _inventory = player.GetComponent<Inventory>();
        }
    }

    private void Update()
    {
        if (_inventory == null)
        {
            return;
        }

        _slotImages[_inventory.currentEquipped].color = new Color(1, 1, 1, 1);
        _slotImages[_inventory.currentEquipped].rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        for (int i = 0; i < _slotImages.Count; i++)
        {
            if (i == _inventory.currentEquipped) continue;
            _slotImages[i].color = new Color(0, 0, 0, 0.9f);
            _slotImages[i].rectTransform.localScale = new Vector3(.6f, .6f, 1f);
        }
    }
}
