using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public bool isEmtpy;
    public List<ParasiteController> enemys;

    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            enemys.Add(this.gameObject.transform.GetChild(i).GetChild(1).GetComponent<ParasiteController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmtpy) return;
        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].dead) return;
        }

        isEmtpy = true;
    }
}
