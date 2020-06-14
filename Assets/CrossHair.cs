using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [Header("15 ~ 55")]
    public Vector2 crossHairOffest;

    public float recoverySpeed;

    public RectTransform[] crossHairs;
    int[] dir;


    public Vector3[] originCrossHairs;

    private void OnEnable()
    {
        if (crossHairs.Length == 0) return;
        StopAllCoroutines();
        StartCoroutine(Recover(crossHairs));
    }

    private void Start()
    {
        dir = new int[transform.childCount];
        crossHairs = new RectTransform[transform.childCount];
        originCrossHairs = new Vector3[transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            crossHairs[i] = this.transform.GetChild(i).GetComponent<RectTransform>();
            originCrossHairs[i] = crossHairs[i].localPosition;
            dir[i] = 0;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StopAllCoroutines();
            MoveCrossHair();
        }

        if(Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StartCoroutine(Recover(crossHairs));
        }
        // shift = recovery speed up
    }

    void MoveCrossHair()
    {
        float addValue = 1.5f;
        Vector3 addVector = Vector3.zero;

        for (int i = 0; i < crossHairs.Length; i++)
        {
            int x = (int)crossHairs[i].localPosition.x;
            int y = (int)crossHairs[i].localPosition.y;

            dir[i] = (x != 0) ? ((x < 0) ? -1 : 1) : (y < 0) ? -1 : 1;
            addVector = (x != 0) ? Vector3.right : Vector3.up;

            crossHairs[i].localPosition += addVector * addValue * dir[i];
        }
    }

    IEnumerator Recover(RectTransform[] crossHairs)
    {
        float lerpSpeed = 0f;
        while(crossHairs[0].localPosition != originCrossHairs[0])
        {
            for(int i = 0; i < crossHairs.Length; i++)
            {
                crossHairs[i].localPosition = Vector3.Lerp(crossHairs[i].localPosition, originCrossHairs[i], lerpSpeed);
            }
            lerpSpeed += Time.deltaTime * recoverySpeed;
            yield return null;
        }
    }
}
