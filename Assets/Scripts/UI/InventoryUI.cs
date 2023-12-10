using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    private TextMeshProUGUI potionsAmount;

    private GridLayoutGroup gridLayout;
    private List<Image> slotImages;

    private void Start()
    {
        //find required refereneces:
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        potionsAmount = GameObject.FindGameObjectWithTag("UI_PAmount").GetComponent<TextMeshProUGUI>();


        gridLayout = GetComponent<GridLayoutGroup>();
        slotImages = new List<Image>();

        var images = gridLayout.GetComponentsInChildren<Image>();

        for (int i = 0, itemid = 0; i < images.Length; i++)
        {
            var x = images[i];

            if (x.gameObject.CompareTag("UI_Item"))
            {
                x.sprite = inventory.itemsData[itemid++].sprite;
                slotImages.Add(images[i]);
            }
        }

        var texts = gridLayout.GetComponentsInChildren<TextMeshProUGUI>();
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

        for (int i = 0; i < slotImages.Count - 1; i++)  //always skip potion
        {
            if (i == inventory.currentEquipped) continue;
            slotImages[i].color = new Color(0, 0, 0, 0.9f);
            slotImages[i].rectTransform.localScale = new Vector3(.6f, .6f, 1f);
        }

        //handle potion
        potionsAmount.text = InvenoryManagment.NumberOfPotions.ToString();

        if(InvenoryManagment.NumberOfPotions == 0)
        {
            slotImages[2].color = new Color(0, 0, 0, 0.9f);
            slotImages[2].rectTransform.localScale = new Vector3(.6f, .6f, 1f);
            potionsAmount.enabled = false;
        }
        else
        {
            potionsAmount.enabled = true;
            slotImages[2].color = new Color(1, 1, 1, .9f);
            slotImages[2].rectTransform.localScale = new Vector3(.8f, .8f, .8f);
        }

    }
}
