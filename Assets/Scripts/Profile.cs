using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Profile : MonoBehaviour
{
    [Header("InputField")]
    public Color targetColor;
    public Color defaultColor;
    public Color hoverColor;

    string defaultText;

    InputField inputField;

    bool isWritting;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
        defaultText = inputField.placeholder.GetComponent<Text>().text;
    }

    public void InputSelect()
    {
        isWritting = true;
        inputField.image.color = targetColor;
        inputField.placeholder.GetComponent<Text>().text = "";
    }

    public void InputDeselect()
    {
        if (inputField.text != "") return;
        isWritting = false;
        inputField.image.color = defaultColor;
        inputField.placeholder.GetComponent<Text>().text = defaultText;
    }

    public void HoverEnter()
    {
        if (isWritting) return;
        inputField.image.color = hoverColor;
    }

    public void HoverExit()
    {
        if (isWritting) return;
        inputField.image.color = defaultColor;
    }

    public void ValueChanged()
    {
        ProfileManager.Instance.Check();

        //play Typing sound
    }
}
