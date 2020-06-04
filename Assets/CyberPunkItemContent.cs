using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CyberPunkItemContent : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image image;
    public GameObject summaryContent;

    public Transform targetTransform;

    Vector3 originPosition;
    Transform originParent;
    Item item;

    public void SetItemInfo(Item _item)
    {
        item = _item;
        itemName.text = _item.name;
        image.sprite = _item.itemImage;
    }

    public void HoverPointerDown()
    {
        summaryContent.SetActive(true);

        originPosition = summaryContent.transform.position;
        originParent = summaryContent.transform.parent;

        summaryContent.transform.SetParent(CyberPunkUIController.Instance.Handler);
        summaryContent.transform.position = Input.mousePosition -
             new Vector3(summaryContent.transform.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0); ;
        summaryContent.transform.GetChild(1).GetComponent<Image>().sprite = image.sprite;

        CyberPunkUIController.Instance.currentContent = this.gameObject.GetComponent<CyberPunkItemContent>();
        CyberPunkUIController.Instance.isItemPicked = true;
    }

    public void HoverMouseDrag()
    {
        summaryContent.transform.position = Input.mousePosition -
             new Vector3(summaryContent.transform.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0);
    }

    public void HoverPointerUp()
    {
        Vector3 targetPosition = targetTransform.localPosition;
        Vector2 offset = new Vector2(targetTransform.GetComponent<RectTransform>().sizeDelta.x * .5f, targetTransform.GetComponent<RectTransform>().sizeDelta.y * .5f);

        summaryContent.transform.parent = targetTransform.parent;
        Vector3 contentPos = summaryContent.transform.localPosition + Vector3.right * 25f;

        if (contentPos.x < -340f)
        {
            WeaponController.Instance.Drop(item);
            Destroy(this.gameObject);
        }

        if (contentPos.x > targetPosition.x - offset.x && contentPos.x < targetPosition.x + offset.x
            && contentPos.y > targetPosition.y - offset.y && contentPos.y < targetPosition.y + offset.y)
        {
            //equipped

            this.gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("not equipped");
        }

        CyberPunkUIController.Instance.isItemPicked = false;
        CyberPunkUIController.Instance.currentContent = null;
        summaryContent.transform.position = originPosition;
        summaryContent.transform.parent = originParent;
        summaryContent.SetActive(false);
    }
}
