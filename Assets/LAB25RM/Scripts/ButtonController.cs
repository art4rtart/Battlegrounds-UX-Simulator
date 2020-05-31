using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using her0in.UI;

public enum ButtonType { None, Scene, Option, Exit }
public class ButtonController : MonoBehaviour
{
    public ButtonType buttonType;

    public int buttonNumber;
    TextMeshProUGUI tmpro;

    [Header("Menu Text Color")]
    public Color defaultColor;
    public Color glowColor;

    [Header("Description Text Color")]
    public TextMeshProUGUI dstmpro;
    public Color dsdefaultColor;
    public Color dsglowColor;

    [Header("UIDissolve")]
    public UIDissolve[] uiDissolve;

    bool clicked = false;

    private void Awake()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
    }

    public void MousePointerEnter()
    {
        if (clicked) return;
        StopAllCoroutines();
        StartCoroutine(ButtonEnableAnimation(1, 0, glowColor, dsglowColor));
    }

    public void MousePointerExit()
    {
        if (clicked) return;
        StopAllCoroutines();
        StartCoroutine(ButtonEnableAnimation(0, 1, defaultColor, dsdefaultColor));
    }

    public void MousePointerClick()
    {
        clicked = true;
        Cursor.visible = false;
        StopAllCoroutines();
        StartCoroutine(TitleSceneController.Instance.FadeOut());
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1f);
        while (!TitleSceneController.Instance.faded) yield return null;

        SceneController sceneController;
        sceneController = FindObjectOfType<SceneController>();

        switch (buttonType)
        {
            case ButtonType.None:
                break;

            case ButtonType.Scene:
                {
                    int sceneIndex = sceneController.scenenumber + buttonNumber;
                    sceneController.LoadScene(sceneIndex);
                    break;
                }

            case ButtonType.Option:
                {
                    sceneController.LoadScene("Option");
                    break;
                }

            case ButtonType.Exit:
                {
                    Application.Quit();
                    break;
                }
        }
        yield return null;
    }

    IEnumerator ButtonEnableAnimation(float _currentValue, float _targetValue, Color _tagetColor, Color _dstargetColor)
    {
        float lerpValue = 0;

        while (lerpValue < 1)
        {
            for (int i = 0; i < uiDissolve.Length; i++)
            {
                uiDissolve[i].effectFactor = Mathf.Lerp(_currentValue, _targetValue, lerpValue * 2.5f);
            }
            tmpro.color = Color.Lerp(tmpro.color, _tagetColor, lerpValue * .5f);
            dstmpro.color = Color.Lerp(dstmpro.color, _dstargetColor, lerpValue * .5f);
            lerpValue += Time.deltaTime * 1.5f;
            yield return null;
        }
        yield return null;
    }

    void OnDisable()
    {
        tmpro.color = defaultColor;
        for (int i = 0; i < uiDissolve.Length; i++)
        {
            uiDissolve[i].effectFactor = 1;
        }
    }
}
