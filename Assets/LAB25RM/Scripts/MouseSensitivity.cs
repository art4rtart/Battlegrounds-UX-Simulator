using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseSensitivity : MonoBehaviour
{
	public static MouseSensitivity Instance { get; private set; }
	[Header("Mouse Sensitivity Text")]
    TextMeshProUGUI mouseText;
    float sensitivity = 0.75f;

    private void Awake()
    {
        mouseText = GetComponent<TextMeshProUGUI>();
		if(PlayerInfoManager.Instance) sensitivity = PlayerInfoManager.Instance.mouseSensitivity;
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            sensitivity -= 0.1f;
			PlayerInfoManager.Instance.mouseSensitivity = sensitivity;
			StopAllCoroutines();
            StartCoroutine(ShowUI(sensitivity));
        }

        else if (Input.GetKeyDown(KeyCode.Period))
        {
            sensitivity += 0.1f;
			PlayerInfoManager.Instance.mouseSensitivity = sensitivity;
            StopAllCoroutines();
            StartCoroutine(ShowUI(sensitivity));
        }
    }

    float alpha = 0;
    float fadeSpeed = 2f;
    IEnumerator ShowUI(float _value)
    {
        FirstPersonController.Instance.mouseManager.XSensitivity = _value;
        FirstPersonController.Instance.mouseManager.YSensitivity = _value;
        if (mouseText) mouseText.text = "<size=20>" + FirstPersonController.Instance.mouseManager.XSensitivity + "</size>" + "\nMouse sensitivity";

        Color textColor = mouseText.color;
        while (alpha < .5f)
        {
            alpha += Time.deltaTime * 2f;
            mouseText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        alpha = .5f;
        yield return new WaitForSeconds(2f);

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 2f;
            mouseText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
    }
}
