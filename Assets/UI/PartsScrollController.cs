using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsScrollController : MonoBehaviour
{
    Animator animator;

    CanvasGroup canvasGroup;
    public bool isPined = false;

    public GameObject handler;
    public GameObject scrollview;
    public TextMeshProUGUI tmpro;

    public GameObject hoverText;


    [Header("Color Settings")]
    public Color defaultColor;
    public Color equippedColor;
    Image image;

    private void OnEnable()
    {
        image.color = defaultColor;
        animator.enabled = true;
    }

    private void Awake()
    {
        if (!scrollview) Debug.Log("Scroll panel should be added.");
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        canvasGroup = scrollview.GetComponent<CanvasGroup>();
        hoverText.SetActive(true);
    }

    public void HoverEnter()
    {
        if (CyberPunkUIController.Instance.isItemPicked &&
            CyberPunkUIController.Instance.currentContent.targetTransform == this.transform
            ) { animator.enabled = false; image.color = equippedColor; }

        if (!CyberPunkUIController.Instance.isItemPicked)
        {
            animator.SetBool("Activate", true);
            hoverText.SetActive(false);
        }
    }

    public void HoverExit()
    {
            image.color = defaultColor;
            animator.enabled = true;
        animator.SetBool("Activate", false);
        if(!isPined) hoverText.SetActive(true);
    }

    public void HoverUp()
    {

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

    public void InitializeScrollViewParent()
    {
        scrollview.transform.SetParent(this.transform);
    }
}
