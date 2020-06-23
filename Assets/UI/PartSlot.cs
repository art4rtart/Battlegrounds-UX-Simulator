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
    public bool isEmpty = true;

    public BGItemContent currentContent;


	public BGItemContent slotBGItem;
	int partType = 0;

    private void Awake()
    {
        panel = GetComponent<Image>();
        currentItemImage = this.transform.GetChild(0).GetComponent<Image>();
		slotBGItem = this.transform.GetChild(0).GetComponent<BGItemContent>();
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
			currentContent = UIController.Instance.selectedContent;

			if (isEmpty)
            {
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
				partType = (int)UIController.Instance.targetItem.partType;

				ItemDetector.Instance.equippedItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

				UIController.Instance.pickedItem = null;
				UIController.Instance.selectedContent = null;
			}
            else
            {
				ItemDetector.Instance.equippedItems.Remove(this.transform.GetChild(0).GetComponent<BGItemContent>().item);
				ItemDetector.Instance.inventoryItems.Add(this.transform.GetChild(0).GetComponent<BGItemContent>().item);

				this.transform.GetChild(0).GetComponent<BGItemContent>().item = currentContent.item;
				slotBGItem.itemImagePanel.sprite = currentContent.item.itemImage;

				ItemDetector.Instance.groundItems.Remove(currentContent.item);
				ItemDetector.Instance.inventoryItems.Remove(currentContent.item);
				ItemDetector.Instance.equippedItems.Add(currentContent.item);

				ItemDetector.Instance.UpdateGroundItem();
				ItemDetector.Instance.UpdateInventoryItems();

				WeaponController.Instance.equippedGun.partsController.WeaponCustomize((int)UIController.Instance.targetItem.partType, this.transform.GetChild(0).GetComponent<BGItemContent>().item.name);
				partType = (int)UIController.Instance.targetItem.partType;
			}

			UIController.Instance.selectedContent = null;
			WeaponController.Instance.weaponAudioSource.PlayOneShot(WeaponController.Instance.gainClip);
		}
    }

    public void SlotPointerDown()
    {
        UIController.Instance.selectedContent = currentContent;
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

			WeaponController.Instance.equippedGun.partsController.WeaponCustomize(partType, "Empty");
			partType = 0;

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

			WeaponController.Instance.equippedGun.partsController.WeaponCustomize(partType, "Empty");
			partType = 0;

			currentContent = null;

            this.transform.GetChild(0).gameObject.SetActive(false);

			isEmpty = true;
        }
    }
}
