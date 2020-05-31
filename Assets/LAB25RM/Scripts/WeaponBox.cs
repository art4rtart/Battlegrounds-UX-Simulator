using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    Quaternion direction;

    public GameObject item;

    public Transform spawnPoint;
    void Start()
    {
        direction = this.transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject obj = Instantiate(item, spawnPoint.position, direction);
            obj.transform.Translate(direction.eulerAngles * 5f);
        }
    }
}
