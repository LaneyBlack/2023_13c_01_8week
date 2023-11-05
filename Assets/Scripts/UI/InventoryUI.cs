using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Invertory script")]
    [SerializeField] Inventory inventory;
    private GridLayoutGroup gridLayout;

    private void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();

        var images = gridLayout.GetComponentsInChildren<Image>();
        var texts = gridLayout.GetComponentsInChildren<Text>();

        for (int i = 0, itemid = 0; i < images.Length; i++)
        {
            var x = images[i];

            if (x.gameObject.CompareTag("UI_Item"))
                x.sprite = inventory.itemsData[itemid++].sprite;
        }

        string pattern = "Keypad|Alpha";
        for (int i = 0, itemid = 0; i < texts.Length; i++)
        {
            var x = texts[i];

            if (x.gameObject.CompareTag("UI_Key"))
                x.text = Regex.Replace(inventory.itemsData[itemid++].keycode.ToString(), pattern, "");
        }
    }
}