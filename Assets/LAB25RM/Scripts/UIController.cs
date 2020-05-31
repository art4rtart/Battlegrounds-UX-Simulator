using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public static UIController Instance {
        get  {
            if (instance != null) return instance;
            instance = FindObjectOfType<UIController>();
            return instance;
        }
    }
    private static UIController instance;

    [Header("Mouse Picker Item")]
    public GameObject pickedItem;
    GameObject raycastingObj;
    public Scrollbar scrollbar;
    public float wheelSensitivity;
    public Transform handler;

    [Header("For Battleground")]
    public ScrollRect groundScrollRect;
    public VerticalLayoutGroup groundVerticalLayoutGroup;

    public ScrollRect inventoryScrollRect;
    public VerticalLayoutGroup inventoryVerticalLayoutGroup;

    public GameObject ground;
    public Transform groundContent;
    public Scrollbar groundScrollbar;
    [HideInInspector] public Vector3 groundSize;
    [HideInInspector] public Vector3 groundPos;

    public GameObject inventory;
    public Transform inventoryContent;
    public Scrollbar inventoryScrollbar;
    [HideInInspector] public Vector3 inventorySize;
    [HideInInspector] public Vector3 inventoryPos;

    Canvas canvas;
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;

    [HideInInspector] public Transform originParent;
    public BGItemContent selectedContent;
    [HideInInspector] public Item targetItem;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);

        inventoryPos = inventory.GetComponent<RectTransform>().position;
        inventorySize = inventory.GetComponent<RectTransform>().sizeDelta * .5f;

        groundPos = ground.GetComponent<RectTransform>().position;
        groundSize = ground.GetComponent<RectTransform>().sizeDelta * .5f;
    }

    void Update()
    {
        if (!PauseMenuController.Instance.isPaused) return;
        if (scrollbar != null) Mathf.Clamp(scrollbar.value += Input.GetAxis("Mouse ScrollWheel") * wheelSensitivity, 0f, 1f);

        List<RaycastResult> results = new List<RaycastResult>();
        pointerEventData.position = Input.mousePosition;
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count == 0) return;
        raycastingObj = results[0].gameObject;
        if (!raycastingObj.CompareTag("Item")) return;
    }

    public void PointerGroundEnter()
    {
        scrollbar = groundScrollbar;
    }

    public void PointerInventoryEnter()
    {
        scrollbar = inventoryScrollbar;
    }

    public void PointerExit()
    {
        scrollbar = null;
    }

    public void IsSelected()
    {
        selectedContent = raycastingObj.GetComponent<BGItemContent>();
        originParent = raycastingObj.transform;
        selectedContent.contentParent = originParent.parent;

        pickedItem = selectedContent.itemSummerizedPanel;
        pickedItem.SetActive(true);

        pickedItem.transform.SetParent(null);
        pickedItem.transform.SetParent(handler);
        pickedItem.transform.position = Input.mousePosition - new Vector3(pickedItem.transform.GetComponent<RectTransform>().sizeDelta.x *.5f, 0, 0);

        targetItem = raycastingObj.GetComponent<BGItemContent>().item;
    }

    public void IsDragging()
    {
        if (!pickedItem) return;
        pickedItem.transform.position = Input.mousePosition - new Vector3(pickedItem.transform.GetComponent<RectTransform>().sizeDelta.x * .5f, 0, 0);
    }

    public void IsDroppedOnInventory()
    {
        bool isOnDropPanel = false;

        if (Input.mousePosition.x > inventoryPos.x - inventorySize.x && inventoryPos.x + inventorySize.x > Input.mousePosition.x
        && Input.mousePosition.y > inventoryPos.y - inventorySize.y && Input.mousePosition.y < inventoryPos.y + inventorySize.y) {
            isOnDropPanel = true;
        }
        else { isOnDropPanel = false; }

        pickedItem.transform.SetParent(null);
        pickedItem.transform.SetParent(originParent);
        pickedItem.SetActive(false);
        originParent = null;

        if (isOnDropPanel)
        {
            // gain Item
            WeaponController.Instance.Gain(targetItem);
            //if (selectedContent.isSlot) {
            //    ItemDetector.Instance.equippedItems.Remove(selectedContent.item);
            //    selectedContent.gameObject.SetActive(false);
            //}

            targetItem = null;
            pickedItem = null;
            selectedContent = null;
        }

        else
        {
            if (!isHoveringWeaponSlot) selectedContent = null;
        }

        ItemDetector.Instance.UpdateGroundItem();
        ItemDetector.Instance.UpdateInventoryItems();
    }

    public void IsDroppedOnGround()
    {
        bool isOnGroundPanel = false;

        if (Input.mousePosition.x > groundPos.x - groundSize.x && groundPos.x + groundSize.x > Input.mousePosition.x
        && Input.mousePosition.y > groundPos.y - groundSize.y && Input.mousePosition.y < groundPos.y + groundSize.y)
        {
            isOnGroundPanel = true;
        }
        else { isOnGroundPanel = false; }

        pickedItem.transform.SetParent(null);
        pickedItem.transform.SetParent(originParent);
        pickedItem.SetActive(false);
        originParent = null;

        if (isOnGroundPanel)
        {
            ItemDetector.Instance.inventoryItems.Remove(pickedItem.transform.parent.GetComponent<BGItemContent>().item);
            ItemDetector.Instance.groundItems.Add(pickedItem.transform.parent.GetComponent<BGItemContent>().item);

            // Drop Item
            WeaponController.Instance.Drop(targetItem);
            //if (selectedContent.isSlot) {
            //    ItemDetector.Instance.equippedItems.Remove(selectedContent.item);
            //    selectedContent.gameObject.SetActive(false);
            //}

            targetItem = null;
            pickedItem = null;
            selectedContent = null;
        }

        else
        {
            if(!isHoveringWeaponSlot) selectedContent = null;
        }

        ItemDetector.Instance.UpdateGroundItem();
        ItemDetector.Instance.UpdateInventoryItems();
    }

    public bool isHoveringWeaponSlot;
}

