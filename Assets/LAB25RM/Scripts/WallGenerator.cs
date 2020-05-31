using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public Vector2 mapSize;
    public GameObject wall;
    public float wallHeight = 5f;
    float wallsize;

    Vector3 currentPivot;
    Vector3 originPivot;
    float buildDir;

    void Start()
    {
        GenerateSideWalls();
    }

    void GenerateSideWalls()
    {
        wallsize = wall.transform.localScale.x;
        currentPivot = new Vector3(-(mapSize.x * .5f), 0, -(mapSize.y * .5f));
        originPivot = currentPivot;
        buildDir = 1f;

        for (int k = 0; k < 2; k++)
        {
            float heightPivot = 0;
            for (int j = 0; j < wallHeight; j++)
            {
                for (int i = 0; i < mapSize.y / wallsize + 1; i++)
                {
                    float offset = Random.Range(-wallsize * .5f, wallsize * .5f);

                    Vector3 pos = currentPivot + Vector3.right * offset + Vector3.up * heightPivot;
                    GameObject leftwall = Instantiate(wall, pos, Quaternion.identity) as GameObject;
                    leftwall.transform.name = "wall";
                    leftwall.transform.SetParent(this.gameObject.transform);
                    currentPivot = new Vector3(currentPivot.x, 0, currentPivot.z + wallsize * buildDir);
                }
                currentPivot = originPivot;

                for (int i = 0; i < mapSize.x / wallsize + 1; i++)
                {
                    float offset = Random.Range(-wallsize * .5f, wallsize * .5f);
                    Vector3 pos = currentPivot + Vector3.forward * offset + Vector3.up * heightPivot;
                    GameObject leftwall = Instantiate(wall, pos, Quaternion.identity);
                    leftwall.transform.name = "wall";
                    leftwall.transform.SetParent(this.gameObject.transform);
                    currentPivot = new Vector3(currentPivot.x + wallsize * buildDir, 0, currentPivot.z);
                }
                heightPivot += wallsize;
                currentPivot = originPivot;
            }

            currentPivot *= -1f;
            originPivot = currentPivot;
            buildDir *= -1f;
        }
    }
}
