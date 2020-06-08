using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyberPunkItemContent : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image image;
    public Sprite itemPartsSprite;

    PartsScrollController partsScrollController;

    public float targetPosX;

    public float index;
    public bool isEmpty;

    private void Start()
    {
        if (isEmpty) targetPosX = -318f;
        partsScrollController = this.transform.parent.GetComponent<PartsScrollController>();
    }

    public void SetItemInfo(Item _item)
    {
        itemName.text = _item.name;
        image.sprite = _item.itemImage;
        itemPartsSprite = _item.itemPartsImage;
    }

    public void ClickItem()
    {
        partsScrollController.currentContent = this.gameObject.GetComponent<CyberPunkItemContent>();
        partsScrollController.MoveToTarget(targetPosX);
    }

    public void SetTarget(int _index)
    {
        index = _index;
        targetPosX = -318f - 100f * _index + 100f;
    }
}
