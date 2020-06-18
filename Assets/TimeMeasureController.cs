using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMeasureController : MonoBehaviour
{
    public float totalGameTime;
    public float customizationTime;


    bool count = false;
    bool goal = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(!Goal.Instance.goal) totalGameTime += Time.deltaTime;
        else Debug.Log("Customization Time : " + customizationTime + " Total Game Time : " + totalGameTime);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            count = !count;
        }

        if (count) customizationTime += Time.deltaTime;
    }
}
