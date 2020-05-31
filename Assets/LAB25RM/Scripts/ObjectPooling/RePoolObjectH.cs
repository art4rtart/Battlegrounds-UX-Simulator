using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePoolObjectH : MonoBehaviour
{
    public bool repoolAfterTime = true;

    [SerializeField]
    private float repoolTime = 5.0f;
    public int index;

    private void OnEnable()
    {
        BeginDestroy();
    }

    public void BeginDestroy()
    {
        if (repoolAfterTime)
        {
            Invoke("Repool", repoolTime);
        }
    }

    public void Repool()
    {
        CancelInvoke();
        gameObject.SetActive(false);
        ObjectPoolerH.instance.SetParentObject(gameObject, ObjectPoolerH.instance.gameObject);
        ObjectPoolerH.instance.DeactivateObject(gameObject, ObjectPoolerH.instance.poolingObject[index].objectName);
    }

    public void SetState(bool _destroyAfterTime, float _destroyTime, int _index)
    {
        repoolAfterTime = _destroyAfterTime;
        repoolTime = _destroyTime;
        index = _index;
    }

}
