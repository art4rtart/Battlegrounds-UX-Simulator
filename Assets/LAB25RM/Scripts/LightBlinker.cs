using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightType { None, Blinker, Alert }

public class LightBlinker : MonoBehaviour
{
    Light pointlight;

    public LightType lightType;

    private void Awake()
    {
        pointlight = GetComponent<Light>();

        if(lightType == LightType.Blinker) StartCoroutine(BlinkLight());
        else if (lightType == LightType.Alert) StartCoroutine(AlertLight());
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            pointlight.intensity = Random.Range(3f, 3.5f);
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator AlertLight()
    {
        float value = 0;
        
        while (true)
        {
            value = Mathf.PingPong(1 + Time.time, 3);

            pointlight.intensity = value;
            yield return null;
        }
    }
}
