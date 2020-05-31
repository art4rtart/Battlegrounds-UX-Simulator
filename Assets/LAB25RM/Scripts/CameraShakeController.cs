using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<CameraShakeController>();
            return instance;
        }
    }
    private static CameraShakeController instance;

    public CameraShake.Properties testProperties;
    public void CameraShake()
    {
        FindObjectOfType<CameraShake>().StartShake(testProperties);
    }
}
