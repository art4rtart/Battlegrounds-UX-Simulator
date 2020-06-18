using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public bool isEmpty;
    public List<Transform> items;
    Projector projector;

    void Start()
    {
        projector = GetComponent<Projector>();
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            items.Add(this.gameObject.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmpty) return;
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].gameObject.activeSelf) return;
        }

        isEmpty = true;
        StartCoroutine(CloseCircle());
    }

    IEnumerator CloseCircle()
    {
        float value = projector.orthographicSize;
        while (value > 0)
        {
            value -= Time.deltaTime * 2f;
            projector.orthographicSize = value;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
