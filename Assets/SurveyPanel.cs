using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SurveyPanel : MonoBehaviour
{
	Animator animator;


	public TextMeshProUGUI sliderValueText;
	public Slider slider;
	public InputField elseInputField;

	public Toggle PlayedToggle;
	public Toggle NotPlayedToggle;

	bool played;
	bool played_1;
	bool played_2;
	bool battleground, warzone, fortnite, elseplayed;

	public Button summitButton;
	public CanvasGroup secondQuestion;
	CanvasGroup canvasGroup;
	bool valueChanged;

	public Text[] levelTexts;
	public Color defaultColor;
	public Color highLightColor;


	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		animator = GetComponent<Animator>();
	}

	public void ShowSurveyPanel()
	{
		animator.SetTrigger("Show");
	}

	public void SummitSurvey()
	{
		// data save

		animator.SetTrigger("Hide");
	}

	public void Played()
	{
		played = true;
		played_1 = !played_1;

		if (played_1)
		{
			secondQuestion.alpha = 1f;
			summitButton.interactable = false;

			PlayedToggle.interactable = false;
			secondQuestion.interactable = true;
			if (NotPlayedToggle.isOn) NotPlayedToggle.isOn = false;
		}
		else
		{
			PlayedToggle.interactable = true;
		}
	}

	public void NotPlayed()
	{
		played = false;
		played_2 = !played_2;

		if (played_2)
		{
			secondQuestion.alpha = 0.1f;
			summitButton.interactable = true;

			NotPlayedToggle.interactable = false;
			secondQuestion.interactable = false;
			if (PlayedToggle.isOn) PlayedToggle.isOn = false;
		}
		else
		{
			NotPlayedToggle.interactable = true;
		}
	}

	public void PlayedBattledGround()
	{
		valueChanged = true;
		battleground = !battleground;
	}

	public void PlayedWarzone()
	{
		valueChanged = true;
		warzone = !warzone;
	}

	public void PlayedFortnite()
	{
		valueChanged = true;
		fortnite = !fortnite;
	}

	public void PlayedElse()
	{
		valueChanged = true;
		elseplayed = !elseplayed;
		if (elseplayed) elseInputField.interactable = true;
	}

	int colorTextIndex;
	public void SliderValueChange()
	{
		float value = slider.value * 100f;
		sliderValueText.text = (value).ToString("N0");
		if(valueChanged) summitButton.interactable = true;

		//for (int i = 0; i < levelTexts.Length; i++)
		//{
		//	levelTexts[i].color = defaultColor;
		//}

		//if (0 <= value && value < 20)
		//{
		//	levelTexts[0].color = highLightColor;
		//}

		//else if (20 < value && value < 40)
		//{
		//	levelTexts[1].color = highLightColor;
		//}
		//else if(40 < value && value < 60)
		//{
		//	levelTexts[2].color = highLightColor;
		//}
		//else if(60 < value && value < 80)
		//{
		//	levelTexts[3].color = highLightColor;
		//}
		//else if (80 < value && value <= 100)
		//{
		//	levelTexts[4].color = highLightColor;
		//}
	}
}
