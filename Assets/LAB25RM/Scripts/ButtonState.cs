using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonState : MonoBehaviour
{
    public Color defaultColor;
    public Color targetColor;
    TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void PointerEnter()
    {
        buttonText.color = targetColor;
    }

    public void PointerExit()
    {
        buttonText.color = defaultColor;
    }
}
