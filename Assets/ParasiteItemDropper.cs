using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteItemDropper : MonoBehaviour
{
    public GameObject[] items;
    public GameObject[] parts;

    int dropQuantity;

    [Range(0, 1)] public float itemDropPercentage;
    [Range(0, 1)] public float partDropPercentage;

    public int maxDropQuantity;
    public float dropRange;

    public bool isDropped = false;
    List<GameObject> dropList = new List<GameObject>();
    public void DropItem()
    {
        if (isDropped) return;

        for (int i = 0; i < items.Length; i++)
        {
            float itemDropValue = Random.Range(0f, 1f);
            if (itemDropValue < itemDropPercentage) dropList.Add(items[i]);
        }

        for (int i = 0; i < parts.Length; i++)
        {
            float partsDropValue = Random.Range(0f, 1f);
            if (partsDropValue < partDropPercentage) dropList.Add(parts[i]);
        }

        float quantity = Mathf.Clamp(dropList.Count, dropList.Count, maxDropQuantity);
        for(int i = 0; i < quantity; i++)
        {
            int index = Random.Range(0, dropList.Count);
            Instantiate(dropList[index], this.transform.position + Vector3.one * Random.Range(-dropRange, dropRange), Quaternion.identity);
            dropList.RemoveAt(index);
        }

        isDropped = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) DropItem();
    }

}
