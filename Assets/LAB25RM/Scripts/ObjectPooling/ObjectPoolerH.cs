using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolingObjects
{
    public string objectName;
    public GameObject targetObject;
    public int numberOfObjects;
    public bool repoolAfterTime;
    public float deactivateTime = 5.0f;
    public List<GameObject> pooledObject;
    public List<GameObject> activeObjects;
}

public class ObjectPoolerH : MonoBehaviour
{
    public static ObjectPoolerH instance;
    public PoolingObjects[] poolingObject;

    private void Awake()
    {
        Application.targetFrameRate = -1;

        instance = this;

        for (int j = 0; j < poolingObject.Length; j++)
        {
            for (int i = 0; i < poolingObject[j].numberOfObjects; i++)
            {
                GameObject obj = (GameObject)Instantiate(poolingObject[j].targetObject);
                obj.SetActive(false);
                obj.AddComponent<RePoolObjectH>();
                obj.GetComponent<RePoolObjectH>().SetState(poolingObject[j].repoolAfterTime, poolingObject[j].deactivateTime, j);
                SetParentObject(obj, this.gameObject);
                poolingObject[j].pooledObject.Add(obj);
            }
        }
    }

    public void SetParentObject(GameObject child, GameObject parent)
    {
        if (parent != null)
            child.transform.SetParent(parent.transform);
    }

    public GameObject GetPooledObject(string objectName)
    {
        int index = IndexFinder(objectName);

        for (int i = 0; i < poolingObject[index].pooledObject.Count; i++)
        {
            if (!poolingObject[index].pooledObject[i].activeInHierarchy)
            {
                poolingObject[index].activeObjects.Add(poolingObject[index].pooledObject[i]);
                return poolingObject[index].pooledObject[i];
            }
        }

        if (poolingObject[index].activeObjects.Count > 0)
        {
            GameObject returnObj = poolingObject[index].activeObjects[0];
            poolingObject[index].activeObjects[0].GetComponent<RePoolObjectH>().Repool();
            poolingObject[index].activeObjects.Add(returnObj);
            return returnObj;
        }
        else
        {
            return null;
        }
    }

    public void DeactivateObject(GameObject obj, string objectName)
    {
        int index = IndexFinder(objectName);

        if (poolingObject[index].activeObjects.Count > 0)
        {
            for (int i = 0; i < poolingObject[index].activeObjects.Count; i++)
            {
                if (obj == poolingObject[index].activeObjects[i])
                {
                    poolingObject[index].activeObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }

    int IndexFinder(string objectName)
    {
        int index = 0;
        for (int i = 0; i < poolingObject.Length; i++)
            if (objectName == poolingObject[i].objectName) index = i;

        return index;
    }
}
