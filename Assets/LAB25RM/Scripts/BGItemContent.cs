using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CurrentList { None, Ground, Inventory, Equip };
public class BGItemContent : MonoBehaviour
{
    public CurrentList currentList;

    [Header("Panel Color Settings")]
    public Color defaultPanelColor;
    public Color targetPanelColor;

    [Header("UI Settings")]
    public Image itemImagePanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;

    public GameObject itemHoverStatePanel;
    public GameObject itemSummerizedPanel;
    public Transform contentParent;
    public Item item;

    public bool isSlot;

    public void HoverEnter()
    {
        if (item) itemSummerizedPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemImage;
        this.transform.GetComponent<Image>().color = targetPanelColor;
        itemHoverStatePanel.SetActive(true);
    }

    public void HoverExit()
    {
        this.transform.GetComponent<Image>().color = defaultPanelColor;
        itemHoverStatePanel.SetActive(false);
    }

    private void OnEnable()
    {
        if(item) itemSummerizedPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemImage;  
    }

    private void OnDisable()
    {
        this.transform.GetComponent<Image>().color = defaultPanelColor;
        if (!PauseMenuController.Instance.isPaused)
        {
            itemHoverStatePanel.SetActive(false);
            itemSummerizedPanel.SetActive(false);
        }
    }
}
