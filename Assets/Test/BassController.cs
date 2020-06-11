using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassController : MonoBehaviour
{
    public Animator animator;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraShakeController.Instance.CameraShake();
            animator.SetTrigger("Bass");
        }
    }
}