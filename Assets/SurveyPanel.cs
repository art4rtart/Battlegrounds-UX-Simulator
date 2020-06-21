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
	bool battleground, warzone, fortnite, elseplayed;

	public Button summitButton;
	public CanvasGroup secondQuestion;

	private void Awake()
	{
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
		secondQuestion.alpha = 1;
		summitButton.interactable = false;
		secondQuestion.interactable = true;

		PlayedToggle.interactable = false;
		NotPlayedToggle.interactable = true;
		if (NotPlayedToggle.isOn) NotPlayedToggle.isOn = false;
	}

	public void NotPlayed()
	{
		played = false;
		secondQuestion.alpha = 0.1f;
		summitButton.interactable = true;
		secondQuestion.interactable = false;

		PlayedToggle.interactable = true;
		if (PlayedToggle.isOn) PlayedToggle.isOn = false;
		NotPlayedToggle.interactable = false;
	}

	public void PlayedBattledGround()
	{
		battleground = !battleground;
	}

	public void PlayedWarzone()
	{
		warzone = !warzone;
	}

	public void PlayedFortnite()
	{
		fortnite = !fortnite;
	}

	public void PlayedElse()
	{
		elseplayed = !elseplayed;
		if (elseplayed) elseInputField.interactable = true;
	}

	public void SliderValueChange()
	{
		sliderValueText.text = slider.value.ToString("N0");
	}
}
