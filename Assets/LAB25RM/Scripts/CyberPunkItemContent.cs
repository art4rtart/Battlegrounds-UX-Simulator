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
    public PartsType partTypeEmpty;
    PartsType partType;

    public float targetPosX;

    public float index;
    public bool isEmpty;

    [Header("Audio")]
    AudioSource audiosource;
    public AudioClip hoverSound;
    public AudioClip selectSound;

    [Header("Color")]
    public Color defaultColor;
    public Color highlightColor;

    RectTransform rectTransform;
    bool hover = false;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        if (isEmpty) targetPosX = -318f;
        partsScrollController = this.transform.parent.GetComponent<PartsScrollController>();
    }

    public void SetItemInfo(Item _item)
    {
        itemName.text = _item.name;
        image.sprite = _item.itemImage;
        itemPartsSprite = _item.itemPartsImage;
        partType = _item.partType;
    }

    public void ClickItem()
    {
        if (isEmpty)
        {
            if(partTypeEmpty == PartsType.Muzzle)
            {
                PartsAddController.Instance.SetMuzzle("Empty");
            }
            if (partTypeEmpty == PartsType.Handle)
            {
                PartsAddController.Instance.SetHandle("Empty");
            }
            if (partTypeEmpty == PartsType.Magazine)
            {
                PartsAddController.Instance.SetMagazine("Empty");
            }
            if (partTypeEmpty == PartsType.Reverse)
            {
                PartsAddController.Instance.SetStock("Empty");
            }
            if (partTypeEmpty == PartsType.Scope)
            {
                PartsAddController.Instance.SetScope("Empty");
            }
        }

        audiosource.PlayOneShot(selectSound);
        partsScrollController.currentContent = this.gameObject.GetComponent<CyberPunkItemContent>();
        partsScrollController.MoveToTarget(targetPosX, partType, itemName.text);
        ItemDetailInfo.Instance.Panel(false);
    }

    public void SetTarget(int _index)
    {
        index = _index;
        targetPosX = -318f - 100f * _index + 100f;
    }

    public void Update()
    {
        if (!hover) return;
        UpdateDetailInfo();
    }

    public void UpdateDetailInfo()
    {
        if (isEmpty) return;
        Vector3 mousePosition = Input.mousePosition;
        if (mousePosition.x > rectTransform.position.x - rectTransform.sizeDelta.x * .5f &&
            mousePosition.x < rectTransform.position.x + rectTransform.sizeDelta.x * .5f &&
            mousePosition.y > rectTransform.position.y - rectTransform.sizeDelta.y * .5f &&
            mousePosition.y < rectTransform.position.y + rectTransform.sizeDelta.y * .5f
            )
        {
            ItemDetailInfo.Instance.Panel(true);
            ItemDetailInfo.Instance.UpdatePosition();
        }

        else
        {
            ItemDetailInfo.Instance.Panel(false);
        }
    }

    public void HoverEnter()
    {
        //audiosource.clip = hoverSound;
        //if(!audiosource.isPlaying) audiosource.Play();
        this.gameObject.GetComponent<Image>().color = highlightColor;

        if (isEmpty) return;
        ItemDetailInfo.Instance.SetDetailInfo(itemName.text, partType, image.sprite);
        hover = true;
    }

    public void HoverExit()
    {
        this.gameObject.GetComponent<Image>().color = defaultColor;

        if (isEmpty) return;
        hover = false;
        ItemDetailInfo.Instance.Panel(false);
    }

    public void OnEnable()
    {
        if(rectTransform) UpdateDetailInfo();
    }

    public void OnDisable()
    {
        this.gameObject.GetComponent<Image>().color = defaultColor;
    }
}
