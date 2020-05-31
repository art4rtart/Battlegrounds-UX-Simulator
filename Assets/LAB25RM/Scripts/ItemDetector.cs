using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public static ItemDetector Instance {
        get {
            if (instance != null) return instance;
            instance = FindObjectOfType<ItemDetector>();
            return instance;
        }
    }
    private static ItemDetector instance;

    public List<Item> groundItems;
    public List<Item> inventoryItems;
    public List<Item> equippedItems;

    public GameObject[] groundPanels;
    public GameObject[] inventoryPanels;
    public Transform itemHolder;

    private void Awake()
    {
        groundPanels = new GameObject[UIController.Instance.groundContent.childCount];
        inventoryPanels = new GameObject[UIController.Instance.inventoryContent.childCount];
        for (int i = 0; i < UIController.Instance.groundContent.childCount; i++)
        {
            groundPanels[i] = UIController.Instance.groundContent.GetChild(i).transform.gameObject;
            inventoryPanels[i] = UIController.Instance.inventoryContent.GetChild(i).transform.gameObject;
        }
    }

    public void UpdateGroundItem()
    {
        for (int i = 0; i < groundPanels.Length; i++)
        {
            groundPanels[i].SetActive(false);
        }

        for (int i = 0; i < groundItems.Count; i++)
        {
            groundPanels[i].SetActive(true);
            groundItems[i].bgItemContent = groundPanels[i].GetComponent<BGItemContent>();
            groundPanels[i].GetComponent<BGItemContent>().item = groundItems[i];
            groundPanels[i].GetComponent<BGItemContent>().itemImagePanel.sprite = groundItems[i].itemImage;
            groundPanels[i].GetComponent<BGItemContent>().itemNameText.text = groundItems[i].name;

            string quantity = (groundItems[i].quantity == 0) ? quantity = "" : groundItems[i].quantity.ToString();
            groundPanels[i].GetComponent<BGItemContent>().itemQuantityText.text = quantity;
        }
    }

    public void UpdateInventoryItems()
    {
        for (int i = 0; i < inventoryPanels.Length; i++)
        {
            inventoryPanels[i].SetActive(false);
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryPanels[i].SetActive(true);
            inventoryItems[i].bgItemContent = groundPanels[i].GetComponent<BGItemContent>();
            inventoryPanels[i].GetComponent<BGItemContent>().item = inventoryItems[i];
            inventoryPanels[i].GetComponent<BGItemContent>().itemImagePanel.sprite = inventoryItems[i].itemImage;
            inventoryPanels[i].GetComponent<BGItemContent>().itemNameText.text = inventoryItems[i].name;

            string quantity = (inventoryItems[i].quantity == 0) ? quantity = "" : inventoryItems[i].quantity.ToString();
            inventoryPanels[i].GetComponent<BGItemContent>().itemQuantityText.text = quantity;
        }
    }
}
