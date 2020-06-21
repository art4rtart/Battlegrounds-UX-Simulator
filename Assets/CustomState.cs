using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomState : MonoBehaviour
{
	public TextMeshProUGUI tmpro;
	CanvasGroup canvasGroup;

	public Color trueColor;
	public Color falseColor;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void UpdateCustomState(bool _isTrue)
	{
		canvasGroup.alpha = 1;
		if (_isTrue) {
			tmpro.text = "custom state:  <color=#61FF49><size=18>RIGHT</color></size>";
		}
		else {
			tmpro.text = "custom state:  <color=#E03838><size=18>WRONG</color></size>";
		}
	}

	public void HideCustomState()
	{
		canvasGroup.alpha = 0f;
		tmpro.text = "custom state:  <color=#E03838><size=18>WRONG</color></size>";
	}
}
