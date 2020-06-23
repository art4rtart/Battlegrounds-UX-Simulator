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

	Material material;

	List<string> playedGames = new List<string>();
	string playerSkilled;

	string _game1 = "배틀그라운드";
	string _game2 = "콜오브듀티:워존";
	string _game3 = "포트나이트";
	string _game4 = "기타 : ";

	public SceneMaster sceneMaster;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		animator = GetComponent<Animator>();
		elseInputField.interactable = false;
	}

	public void ShowSurveyPanel()
	{
		animator.SetTrigger("Show");

		material = GetComponent<Image>().material;
		StartCoroutine(FadeInBlur());
	}

	public void SummitSurvey()
	{
		ProfileManager.Instance.audioSource.PlayOneShot(ProfileManager.Instance.selectClip);
		ProfileManager.Instance.registrationText.color = ProfileManager.Instance.defaultColor;
		ProfileManager.Instance.playText.color = ProfileManager.Instance.highlightColor;

		// data save
		string _played = played ? "플레이 경험이 있음" : "플레이 경험이 없음";
		string _game = "";
		string _playerSkilled = playerSkilled + " : " + (slider.value * 100f).ToString("N0");

		for(int i = 0; i < playedGames.Count; i++)
		{
			_game += playedGames[i].ToString();
			if(i != playedGames.Count - 1) _game += ", ";
		}

		string elseGame = elseInputField.text;
		if (elseGame != "") {
			if (playedGames.Count == 0)
				_game += elseGame;
			else
				_game += ", " + elseGame;
		}

		if(played) PlayerInfoManager.Instance.SavePlayerSurveyData(_played, _game, _playerSkilled);
		else PlayerInfoManager.Instance.SavePlayerSurveyData(_played, _played, _played);

		animator.SetTrigger("Hide");
		material = GetComponent<Image>().material;
		StartCoroutine(FadeOutBlur());
	}

	IEnumerator FadeOutBlur()
	{
		float value = material.GetFloat("_Size"); ;

		while(value > 0)
		{
			value -= Time.deltaTime * 2f;
			material.SetFloat("_Size", value);
			yield return null;
		}
	}

	IEnumerator FadeInBlur()
	{
		float value = 0f;

		while (value < 2.5f)
		{
			value += Time.deltaTime * 2f;
			material.SetFloat("_Size", value);
			yield return null;
		}
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
		if (battleground) playedGames.Add(_game1);
		else playedGames.Remove(_game1);
	}

	public void PlayedWarzone()
	{
		valueChanged = true;
		warzone = !warzone;
		if (warzone) playedGames.Add(_game2);
		else playedGames.Remove(_game2);
	}

	public void PlayedFortnite()
	{
		valueChanged = true;
		fortnite = !fortnite;
		if (fortnite) playedGames.Add(_game3);
		else playedGames.Remove(_game3);
	}

	public void PlayedElse()
	{
		valueChanged = true;
		elseplayed = !elseplayed;
		if (elseplayed) elseInputField.interactable = true;
		else elseInputField.interactable = false;
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

		if (0 <= value && value < 20)
		{
			playerSkilled = "최하 (Newb)";
		}

		else if (20 < value && value < 40)
		{
			playerSkilled = "하 (low class)";
		}
		else if (40 < value && value < 60)
		{
			playerSkilled = "중 (Intermediate)";
		}
		else if (60 < value && value < 80)
		{
			playerSkilled = "상 (Advance)";
		}
		else if (80 < value && value <= 100)
		{
			playerSkilled = "최상 (Expert)";
		}
	}
}
