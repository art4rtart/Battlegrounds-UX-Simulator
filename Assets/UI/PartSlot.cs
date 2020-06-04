using UnityEngine;
using UnityEngine.UI;

public enum SlotType { None, Muzzle, Handle, Magazine, Reverse, Scope };
public class PartSlot : MonoBehaviour
{
    Image panel;
    Image currentItemImage;

    public SlotType slotType;

    [Header("Panel Color Settings")]
    public Color defaultPanelColor;
    public Color targetPanelColor;

    bool isDropable;
    bool isEmpty = true;

    public BGItemContent currentContent;

    private void Awake()
    {
        panel = GetComponent<Image>();
        currentItemImage = this.transform.GetChild(0).GetComponent<Image>();
    }

    bool isHovering;

    public void HoverEnter()
    {
        if (!UIController.Instance.pickedItem) return;
        isHovering = true;
        if ((int)slotType == (int)UIController.Instance.targetItem.partType)
        {
            panel.color = targetPanelColor;
            isDropable = true;
            // add in equippeed
        }
        else { panel.color = defaultPanelColor; isDropable = false; }
        UIController.Instance.isHoveringWeaponSlot = true;
    }

    public void HoverExit()
    {
        isHovering = false;
        panel.color = defaultPanelColor;
        UIController.Instance.isHoveringWeaponSlot = false;
    }

    private void Update()
    {
        if (!isHovering) return;

        if (Input.GetMouseButtonUp(0) && isDropable && UIController.Instance.selectedContent != null)
        {
            if (isEmpty)
            {
                currentContent = UIController.Instance.selectedContent;

                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(0).GetComponent<BGItemContent>().itemImagePanel.sprite =
                    UIController.Instance.pickedItem.transform.GetChild(1).GetComponent<Image>().sprite;

                this.transform.GetChild(0).GetComponent<BGItemContent>().item = currentContent.item;

                if (currentContent.currentList == CurrentList.Ground)
                {
                    UIController.Instance.targetItem.gameObject.SetActive(false);
                    ItemDetector.Instance.groundItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                    ItemDetector.Instance.UpdateGroundItem();
                }

                else if (currentContent.currentList == CurrentList.Inventory)
                {
                    ItemDetector.Instance.inventoryItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                    ItemDetector.Instance.UpdateInventoryItems();
                }

                if ((int)slotType == (int)UIController.Instance.targetItem.partType)
                {
                    panel.color = targetPanelColor;
                    isDropable = true;
                }

                isEmpty = false;

                WeaponController.Instance.equippedGun.partsController.WeaponCustomize((int)UIController.Instance.targetItem.partType, this.transform.GetChild(0).GetComponent<BGItemContent>().item.name);
                ItemDetector.Instance.equippedItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
            }
            else
            {
                ItemDetector.Instance.equippedItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

                if (currentContent.currentList == CurrentList.Ground)
                {
                    this.transform.GetChild(0).GetComponent<BGItemContent>().item.gameObject.SetActive(true);
                    ItemDetector.Instance.groundItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                }
                else if (currentContent.currentList == CurrentList.Inventory)
                {
                    ItemDetector.Instance.inventoryItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                }

                currentContent = UIController.Instance.selectedContent;
                this.transform.GetChild(0).GetComponent<BGItemContent>().item = currentContent.item;
                this.transform.GetChild(0).GetComponent<BGItemContent>().itemImagePanel.sprite =
                    UIController.Instance.pickedItem.transform.GetChild(1).GetComponent<Image>().sprite;

                if (currentContent.currentList == CurrentList.Ground)
                {
                    this.transform.GetChild(0).GetComponent<BGItemContent>().item.gameObject.SetActive(false);
                    ItemDetector.Instance.groundItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                }
                else if (currentContent.currentList == CurrentList.Inventory)
                {
                    ItemDetector.Instance.inventoryItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
                }

                WeaponController.Instance.equippedGun.partsController.WeaponCustomize((int)UIController.Instance.targetItem.partType, this.transform.GetChild(0).GetComponent<BGItemContent>().item.name);
                ItemDetector.Instance.equippedItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

                ItemDetector.Instance.UpdateGroundItem();
                ItemDetector.Instance.UpdateInventoryItems();
            }
            UIController.Instance.pickedItem = null;
            UIController.Instance.selectedContent = null;
        }
    }

    public void SlotPointerDown()
    {
        UIController.Instance.selectedContent = currentContent;
        Debug.Log(currentContent.item);
        UIController.Instance.originParent = this.transform;
        UIController.Instance.selectedContent.contentParent = this.transform;

        UIController.Instance.pickedItem = currentContent.itemSummerizedPanel;
        UIController.Instance.pickedItem.SetActive(true);

        UIController.Instance.pickedItem.transform.SetParent(null);
        UIController.Instance.pickedItem.transform.SetParent(UIController.Instance.handler);
        UIController.Instance.pickedItem.transform.position = Input.mousePosition - new Vector3(UIController.Instance.pickedItem.transform.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0);

        UIController.Instance.targetItem = currentContent.item;
    }

    public void SlotDrag()
    {
        if (!UIController.Instance.pickedItem) return;
        UIController.Instance.pickedItem.transform.position = 
            Input.mousePosition - new Vector3(UIController.Instance.pickedItem.transform.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0);
    }

    public void SlotPointerUp()
    {
        Vector3 groundPosition = UIController.Instance.groundPos;
        Vector3 groundSize = UIController.Instance.groundSize;

        Vector3 inventoryPosition = UIController.Instance.inventoryPos;
        Vector3 inventorySize = UIController.Instance.inventorySize;

        UIController.Instance.pickedItem.transform.SetParent(null);
        UIController.Instance.pickedItem.transform.SetParent(UIController.Instance.originParent);
        UIController.Instance.pickedItem.SetActive(false);
        UIController.Instance.originParent = null;

        if (Input.mousePosition.x > groundPosition.x - groundSize.x && groundPosition.x + groundSize.x > Input.mousePosition.x
        && Input.mousePosition.y > groundPosition.y - groundSize.y && Input.mousePosition.y < groundPosition.y + groundSize.y)
        {
            ItemDetector.Instance.equippedItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
            ItemDetector.Instance.groundItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

            WeaponController.Instance.Drop(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

            UIController.Instance.targetItem = null;
            UIController.Instance.pickedItem = null;
            UIController.Instance.selectedContent = null;

            ItemDetector.Instance.UpdateGroundItem();
            ItemDetector.Instance.UpdateInventoryItems();

            currentContent = null;

            this.transform.GetChild(0).gameObject.SetActive(false);

            isEmpty = true; 
        }

        else if (Input.mousePosition.x > inventoryPosition.x - inventorySize.x && inventoryPosition.x + inventorySize.x > Input.mousePosition.x
        && Input.mousePosition.y > inventoryPosition.y - inventorySize.y && Input.mousePosition.y < inventoryPosition.y + inventorySize.y)
        {
            ItemDetector.Instance.equippedItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
            ItemDetector.Instance.inventoryItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

            UIController.Instance.targetItem = null;
            UIController.Instance.pickedItem = null;
            UIController.Instance.selectedContent = null;

            ItemDetector.Instance.UpdateGroundItem();
            ItemDetector.Instance.UpdateInventoryItems();

            currentContent = null;

            this.transform.GetChild(0).gameObject.SetActive(false);

            isEmpty = true;
        }
    }
}
