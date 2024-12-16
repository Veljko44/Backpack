using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public Text backpackText;
    private bool isMouseOver = false;
    private ServerManager serverManager;

    private void Start()
    {
        serverManager = FindObjectOfType<ServerManager>();
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        UpdateUI();
        StartCoroutine(serverManager.SendItemData(item, "add"));
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        UpdateUI();
        StartCoroutine(serverManager.SendItemData(item, "remove"));
    }
    private void UpdateUI()
    {
        if (isMouseOver)
        {
            if (items.Count == 0)
            {
                backpackText.text = "Backpack:\n Empty";
            }
            else
            {
                backpackText.text = "Backpack:\n";
                foreach (var item in items)
                {
                    backpackText.text += $"{item.itemName} (ID: {item.id})\n";
                }
            }
        }
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
        backpackText.gameObject.SetActive(true);
        UpdateUI();
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        backpackText.gameObject.SetActive(false);
    }
}