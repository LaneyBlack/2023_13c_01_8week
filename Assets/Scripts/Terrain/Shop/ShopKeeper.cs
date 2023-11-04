using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject _shopUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _shopUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _shopUI.SetActive(false);
        }
    }
}