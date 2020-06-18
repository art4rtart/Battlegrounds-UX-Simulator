using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailInfo : MonoBehaviour
{
    public static ItemDetailInfo Instance {
        get {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<ItemDetailInfo>();
            return instance;
        }
    }
    private static ItemDetailInfo instance;

    RectTransform rectTransform;
    Vector3 offset;
    CanvasGroup canvasGroup;
    Animator animator;
    public bool isActive;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        offset = rectTransform.sizeDelta * .5f;
    }

    public void UpdatePosition()
    {
        rectTransform.position = Input.mousePosition + offset;
    }

    public void Panel(bool _isShowing)
    {
        if (_isShowing) animator.SetBool("Fade", true);
        else animator.SetBool("Fade", false);
    }

    public TextMeshProUGUI partsName;
    public Image partsImage;
    public TextMeshProUGUI partsDetailInfo;

    public void SetDetailInfo(string _name, PartsType _partsType, Sprite _sprite)
    {
        partsName.text = _name + "\n" + "<size=12.5 ><color=#8C8C8C>" + _partsType.ToString() + "</size></color>";
        partsImage.sprite = _sprite;
        partsDetailInfo.text = DetailInfo(_name);
    }

    string DetailInfo(string _name)
    {
        string info = "You can see object with 8 times scope.You can see object with 8 times scope.You can see object with 8 times scope.";

        return info;
    }
}
