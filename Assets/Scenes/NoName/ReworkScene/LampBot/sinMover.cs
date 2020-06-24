using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sinMover : MonoBehaviour
{
    Vector3 originPosition;
    public float MoveDistance = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(new Vector3(originPosition.x , 
        originPosition.y + Mathf.Sin(MoveDistance+Time.time)
        ,originPosition.z),transform.rotation);
    }
}
