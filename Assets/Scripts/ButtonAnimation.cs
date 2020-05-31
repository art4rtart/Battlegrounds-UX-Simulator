using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonAnimation : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HoverEnter()
    {
        animator.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        animator.SetBool("Hover", false);
    }
}
