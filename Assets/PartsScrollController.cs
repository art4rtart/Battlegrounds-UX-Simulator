using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsScrollController : MonoBehaviour
{
    Animator animator;

    CanvasGroup canvasGroup;
    bool isPined = false;

    public GameObject handler;
    public GameObject scrollview;
    public TextMeshProUGUI tmpro;

    private void Awake()
    {
        if (!scrollview) Debug.Log("Scroll panel should be added.");
        animator = GetComponent<Animator>();
        canvasGroup = scrollview.GetComponent<CanvasGroup>();
    }

    public void HoverEnter()
    {
        Debug.Log("Enter");
        animator.SetBool("Activate", true);
    }

    public void HoverExit()
    {
        Debug.Log("Exit");
        animator.SetBool("Activate", false);
    }

    public void HoverClick()
    {
        isPined = !isPined;
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
        animator.SetBool("Pined", isPined);

        if (isPined)
        {
            tmpro.text = "SELECT";
            scrollview.transform.SetParent(handler.transform);
        }

        else
        {
            tmpro.text = "SCROLL";
            scrollview.transform.SetParent(this.transform);
        }
    }

    public void ScrollViewDown()
    {
        scrollview.GetComponent<ScrollRect>().enabled = false;
    }

    public void ScrollViewUp()
    {
        scrollview.GetComponent<ScrollRect>().enabled = true;
    }
}
