using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyPanelAfter : MonoBehaviour
{
	public GameObject surveyPanel;

	GameObject[] questions;
	public int questionIndex = -1;

	public Button nextButton;
	public Button summitButton;
	Animator animator;

	public InputField feedBackInputField;
	AudioSource audioSource;
	public AudioClip selectClip;
	private void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();

		questions = new GameObject[surveyPanel.transform.childCount];

		nextButton.interactable = false;

		for (int i = 0; i < questions.Length; i++)
		{
			questions[i] = surveyPanel.transform.GetChild(i).gameObject;
			questions[i].SetActive(false);
		}
		questions[0].SetActive(true);
	}

	public void ShowQuestion()
	{
		questions[questionIndex].SetActive(false);
		questionIndex++;

		questionIndex = Mathf.Clamp(questionIndex, 0, questions.Length - 1);
		questions[questionIndex].SetActive(true);
		animator.ResetTrigger("Show");
	}

	public void NextButtonClick()
	{
		audioSource.PlayOneShot(selectClip);
		if (!animator.enabled) animator.enabled = true;
		animator.SetTrigger("Show");
		nextButton.interactable = false;

		if (questionIndex == questions.Length - 2)
		{
			nextButton.gameObject.SetActive(false);
			summitButton.interactable = true;
			PlayerInfoManager.Instance.SavePlayerSurveyDataAfterGame(battleGroundLevel, ourGameLevel, verseLevel,
			battleGroundPerfer, ourGamePerfer, versePerfer, intuition, aesthetic, efficiency, feedback);
		}
	}

	SceneMaster sceneMaster;

	string battleGroundLevel, ourGameLevel, verseLevel, battleGroundPerfer, ourGamePerfer, versePerfer,
		intuition, aesthetic, efficiency;
	public void SummitButtonClick()
	{
		audioSource.PlayOneShot(selectClip);

		// Load to credit
		sceneMaster = FindObjectOfType<SceneMaster>();
		sceneMaster.LoadCredit();

		PlayerInfoManager.Instance.SaveDataSendMail();
	}

	// level
	[Header("Level Estimate")]
	public Slider BattleGroundLevelSlider;
	public Text BattleGroundLevelSliderState;
	public Slider OurGameLevelSlider;
	public Text OurGameLevelSliderState;
	public Slider VerseLevelSlider;
	string levelVerseState;
	bool battlegroundLevelValueChanged;
	bool ourgameLevelValueChanged;
	bool verseLevelValueChanged;

	[Header("Perference Estimate")]
	public Slider BattleGroundPerferlSlider;
	public Text BattleGroundPerferSliderState;
	public Slider OurGamePerferSlider;
	public Text OurGamePerferSliderState;
	public Slider VersePerferSlider;
	string perferVerseState;
	bool battlegroundPerferValueChanged;
	bool ourgamePerferlValueChanged;
	bool versePerferValueChanged;

	public void levelSliderValueChangeBattleGround()
	{
		battlegroundLevelValueChanged = true;
		if (battlegroundLevelValueChanged && ourgameLevelValueChanged) nextButton.interactable = true;
		float value = BattleGroundLevelSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			BattleGroundLevelSliderState.text = "매우 어려움";
		}

		else if (20 < value && value < 40)
		{
			BattleGroundLevelSliderState.text = "어려움";
		}
		else if (40 < value && value < 60)
		{
			BattleGroundLevelSliderState.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			BattleGroundLevelSliderState.text = "쉬움";
		}
		else if (80 < value && value <= 100)
		{
			BattleGroundLevelSliderState.text = "매우 쉬움";
		}

		battleGroundLevel = BattleGroundLevelSliderState.text + " (" + (BattleGroundLevelSlider.value * 100f).ToString("N0") + ")";
	}

	public void levelSliderValueChangeOurGame()
	{
		ourgameLevelValueChanged = true;
		if (battlegroundLevelValueChanged && ourgameLevelValueChanged) nextButton.interactable = true;
		float value = OurGameLevelSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			OurGameLevelSliderState.text = "매우 어려움";
		}

		else if (20 < value && value < 40)
		{
			OurGameLevelSliderState.text = "어려움";
		}
		else if (40 < value && value < 60)
		{
			OurGameLevelSliderState.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			OurGameLevelSliderState.text = "쉬움";
		}
		else if (80 < value && value <= 100)
		{
			OurGameLevelSliderState.text = "매우 쉬움";
		}
		ourGameLevel = OurGameLevelSliderState.text + " (" + (OurGameLevelSlider.value * 100f).ToString("N0") + ")";
	}

	public void levelSliderValueChangeVerse()
	{
		verseLevelValueChanged = true;
		float value = VerseLevelSlider.value * 100f;
		if (verseLevelValueChanged) nextButton.interactable = true;

		if (0 <= value && value < 20)
		{
			levelVerseState = "매우 좋음[배그]";
		}

		else if (20 < value && value < 40)
		{
			levelVerseState = "좋음[배그]";
		}
		else if (40 < value && value < 60)
		{
			levelVerseState = "비슷";
		}
		else if (60 < value && value < 80)
		{
			levelVerseState = "좋음[새로운UI]";
		}
		else if (80 < value && value <= 100)
		{
			levelVerseState = "매우 좋음[새로운UI]";
		}

		verseLevel = levelVerseState + " (" + (VerseLevelSlider.value * 100f).ToString("N0") + ")";
	}

	public void perferSliderValueChangeBattleGround()
	{
		battlegroundPerferValueChanged = true;
		if (battlegroundPerferValueChanged && ourgamePerferlValueChanged) nextButton.interactable = true;
		float value = BattleGroundPerferlSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			BattleGroundPerferSliderState.text = "매우 나쁨";
		}

		else if (20 < value && value < 40)
		{
			BattleGroundPerferSliderState.text = "나쁨";
		}
		else if (40 < value && value < 60)
		{
			BattleGroundPerferSliderState.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			BattleGroundPerferSliderState.text = "좋음";
		}
		else if (80 < value && value <= 100)
		{
			BattleGroundPerferSliderState.text = "매우 좋음";
		}

		battleGroundPerfer = BattleGroundPerferSliderState.text + " (" + (BattleGroundPerferlSlider.value * 100f).ToString("N0") + ")";
	}

	public void perferSliderValueChangeOurGame()
	{
		ourgamePerferlValueChanged = true;
		if (battlegroundPerferValueChanged && ourgamePerferlValueChanged) nextButton.interactable = true;
		float value = OurGamePerferSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			OurGamePerferSliderState.text = "매우 나쁨";
		}

		else if (20 < value && value < 40)
		{
			OurGamePerferSliderState.text = "나쁨";
		}
		else if (40 < value && value < 60)
		{
			OurGamePerferSliderState.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			OurGamePerferSliderState.text = "좋음";
		}
		else if (80 < value && value <= 100)
		{
			OurGamePerferSliderState.text = "매우 좋음";
		}

		ourGamePerfer = OurGamePerferSliderState.text + " (" + (OurGamePerferSlider.value * 100f).ToString("N0") + ")";
	}

	public void perferSliderValueChangeVerse()
	{
		versePerferValueChanged = true;
		float value = VersePerferSlider.value * 100f;
		if (versePerferValueChanged) nextButton.interactable = true;

		if (0 <= value && value < 20)
		{
			perferVerseState = "매우 좋음[배그]";
		}

		else if (20 < value && value < 40)
		{
			perferVerseState = "좋음[배그]";
		}
		else if (40 < value && value < 60)
		{
			perferVerseState = "비슷";
		}
		else if (60 < value && value < 80)
		{
			perferVerseState = "좋음[새로운UI]";
		}
		else if (80 < value && value <= 100)
		{
			perferVerseState = "매우 좋음[새로운UI]";
		}

		versePerfer = levelVerseState + " (" + (VersePerferSlider.value * 100f).ToString("N0") + ")";
	}

	[Header("UI Estimate")]
	public Slider intuitionSlider;
	public Slider aestheticSlider;
	public Slider efficiencySlider;
	bool valueChanged_1;
	bool valueChanged_2;
	bool valueChanged_3;
	public Text text_1;
	public Text text_2;
	public Text text_3;
	public void Intuition()
	{
		valueChanged_1 = true;
		if (valueChanged_1 && valueChanged_2 && valueChanged_3) nextButton.interactable = true;
		float value = intuitionSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			text_1.text = "매우 낮음";
		}
		else if (20 < value && value < 40)
		{
			text_1.text = "낮음";
		}
		else if (40 < value && value < 60)
		{
			text_1.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			text_1.text = "높음";
		}
		else if (80 < value && value <= 100)
		{
			text_1.text = "매우 높음";
		}

		intuition = text_1.text + "(" + (intuitionSlider.value * 100f).ToString("N0") + ")";                 // 직관성
	}

	public void Aesthetic()
	{
		valueChanged_2 = true;
		if (valueChanged_1 && valueChanged_2 && valueChanged_3) nextButton.interactable = true;
		float value = aestheticSlider.value * 100f;

		if (0 <= value && value < 20)
		{
			text_2.text = "매우 낮음";
		}
		else if (20 < value && value < 40)
		{
			text_2.text = "낮음";
		}
		else if (40 < value && value < 60)
		{
			text_2.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			text_2.text = "높음";
		}
		else if (80 < value && value <= 100)
		{
			text_2.text = "매우 높음";
		}

		aesthetic = text_2.text + "(" + (aestheticSlider.value * 100f).ToString("N0") + ")";                 // 심미성
	}

	public void Efficiency()
	{
		valueChanged_3 = true;
		if (valueChanged_1 && valueChanged_2 && valueChanged_3) nextButton.interactable = true;
		float value = efficiencySlider.value * 100f;

		if (0 <= value && value < 20)
		{
			text_3.text = "매우 낮음";
		}
		else if (20 < value && value < 40)
		{
			text_3.text = "낮음";
		}
		else if (40 < value && value < 60)
		{
			text_3.text = "보통";
		}
		else if (60 < value && value < 80)
		{
			text_3.text = "높음";
		}
		else if (80 < value && value <= 100)
		{
			text_3.text = "매우 높음";
		}

		efficiency = text_3.text + "(" + (efficiencySlider.value * 100f).ToString("N0") + ")";        // 효율성
	}

	string feedback;
	bool feedbackValueChange;
	public void InputFieldValueChange()
	{
		feedbackValueChange = true;
		if (feedbackValueChange) nextButton.interactable = true;
		feedback = feedBackInputField.text;
	}
}
