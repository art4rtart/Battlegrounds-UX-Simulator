using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteGenerator : MonoBehaviour
{
    public Transform[] spawnPos;
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            GenerateParasite();
    }

    void GenerateParasite()
    {
        GameObject parasite = ObjectPoolerH.instance.GetPooledObject("Parasite");
        parasite.transform.position = spawnPos[Random.Range(0, spawnPos.Length)].position;
        parasite.transform.rotation = Quaternion.identity;
        parasite.SetActive(true);
    }
}
