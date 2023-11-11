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
    private List<Image> slotImages;

    private void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        slotImages = new List<Image>();

        var images = gridLayout.GetComponentsInChildren<Image>();
        var texts = gridLayout.GetComponentsInChildren<Text>();

        for (int i = 0, itemid = 0; i < images.Length; i++)
        {
            var x = images[i];

            if (x.gameObject.CompareTag("UI_Item"))
            {
                x.sprite = inventory.itemsData[itemid++].sprite;
                slotImages.Add(images[i]);
            }
        }

        string pattern = "Keypad|Alpha";
        for (int i = 0, itemid = 0; i < texts.Length; i++)
        {
            var x = texts[i];

            if (x.gameObject.CompareTag("UI_Key"))
                x.text = Regex.Replace(inventory.itemsData[itemid++].keycode.ToString(), pattern, "");
        }
    }

    private void Update()
    {
        slotImages[inventory.currentEquipped].color = new Color(1, 1, 1, 1);
        slotImages[inventory.currentEquipped].rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i == inventory.currentEquipped) continue;
            slotImages[i].color = new Color(0, 0, 0, 0.9f);
            slotImages[i].rectTransform.localScale = new Vector3(.6f, .6f, 1f);
        }

    }
}
