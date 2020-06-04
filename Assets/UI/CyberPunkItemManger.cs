using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberPunkItemManger : MonoBehaviour
{
    public static CyberPunkItemManger Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<CyberPunkItemManger>();
            return instance;
        }
    }
    private static CyberPunkItemManger instance;


    public Transform itemHolder;
    public List<Item> inventory;

    [Header("Scope/Muzzle/Handle/Stock/Magazine")]
    public Transform[] parent;

    public GameObject content;

    public void AddItemToInvenotry(Item _item)
    {
        inventory.Add(_item);

        if (_item.partType == PartsType.None) return;

        GameObject obj = Instantiate(content, content.transform.position, Quaternion.identity);
        int parentIndex = 0;

        switch (_item.partType)
        {
            case PartsType.Scope:
                parentIndex = 0;
                break;
            case PartsType.Muzzle:
                parentIndex = 1;
                break;
            case PartsType.Handle:
                parentIndex = 2;
                break;
            case PartsType.Reverse:
                parentIndex = 3;
                break;
            case PartsType.Magazine:
                parentIndex = 4;
                break;
        }
        obj.GetComponent<CyberPunkItemContent>().targetTransform = parent[parentIndex].parent.parent.parent;
        obj.transform.SetParent(parent[parentIndex]);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<CyberPunkItemContent>().SetItemInfo(_item);
    }
}
