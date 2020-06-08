using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsScrollController : MonoBehaviour
{
    RectTransform rectTransform;

    int count = 0;
    bool isMoved = false;

    Vector3 originTransform;

    int contentCount;
    float originX;
    float x = 0;

    public RectTransform hoverBoundingBox;

    public void OnEnable()
    {
        contentCount = this.transform.childCount;

        for(int i = 2; i < contentCount; i++)
        {
            CyberPunkItemContent cpContent = transform.GetChild(i).GetComponent<CyberPunkItemContent>();
            cpContent.SetTarget(i);
        }
    }

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originTransform = rectTransform.localPosition;
        originX = rectTransform.anchoredPosition.x;
        x = originX;
    }

    bool scrollValueChanged = false;


    bool isActive = false;

    bool IsHovering()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (mousePosition.x > hoverBoundingBox.position.x - hoverBoundingBox.sizeDelta.x * .5f
            && mousePosition.x < hoverBoundingBox.position.x + hoverBoundingBox.sizeDelta.x * .5f
            && mousePosition.y > hoverBoundingBox.position.y - hoverBoundingBox.sizeDelta.y * .5f
            && mousePosition.y < hoverBoundingBox.position.y + hoverBoundingBox.sizeDelta.y * .5f)
            isActive = true;
        else isActive = false;

        return isActive;
    }

    public void Update()
    {
        if (!IsHovering()) return;

        float wheel = Input.GetAxis("Mouse ScrollWheel") * 1000f;

        if(wheel != 0)
        {
            x = Mathf.Clamp(x -= wheel, originX - contentCount * 100f + 200f, originX);
            StopAllCoroutines();
            StartCoroutine(ScrollToTarget(x));
            wheel = 0;
        }
    }

    public void MoveToTarget(float _targetPosX)
    {
        StopAllCoroutines();
        StartCoroutine(ScrollToTarget(_targetPosX));
    }

    IEnumerator ScrollToTarget(float _targetPosX)
    {
        float lerpSpeed = 0f;

        float currentPosX = rectTransform.anchoredPosition.x;
        while (currentPosX != _targetPosX)
        {
            currentPosX = Mathf.Lerp(currentPosX, _targetPosX, lerpSpeed);
            lerpSpeed += Time.deltaTime * .5f;
            rectTransform.anchoredPosition = new Vector2(currentPosX, rectTransform.anchoredPosition.y);
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2((int)_targetPosX  + 1f, rectTransform.anchoredPosition.y);
        scrollValueChanged = false;

        // play sound

        // equip effect
        if(partsImage.sprite != null && currentContent.itemPartsSprite != null) partsImage.sprite = currentContent.itemPartsSprite;
    }

    public Image partsImage;
    [HideInInspector]
    public CyberPunkItemContent currentContent;
}
