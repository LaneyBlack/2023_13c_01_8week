using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public GameObject shopUI;

    private bool canInteract = false;

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            shopUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}