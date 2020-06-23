﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance
    {
        get {
            if (instance != null) return instance;
            instance = FindObjectOfType<ProfileManager>();
            return instance;
        }
    }
    private static ProfileManager instance;


    public int isFilled;
    public bool isAgreed;

    public InputField nameInput;
    public InputField ageInput;

    public TextMeshProUGUI thankyouText;

    [Header("Toggle")]
    public Toggle toggle;
	public Toggle man;
	public Toggle woman;
    public Color activateColor;
    public Color deactivateColor;

    [Header("Button")]
    public Button button;
    public Color hoverEnterColor;
    public Color hoverExitColor;

	public Button battlegroundButton;
	public Button cyberneticButton;

    public bool isRegistered = false;

    public Animator animator;
	public SurveyPanel surveyPanelBefore;
	public GameObject surveyPanelAfter;

	public AudioClip selectClip;
	public AudioClip hoverClip;
	[HideInInspector] public AudioSource audioSource;
	public Color buttonDisableColor;


	[Header("Pipeline")]
	public TextMeshProUGUI registrationText;
	public TextMeshProUGUI playText;
	public TextMeshProUGUI surveyText;
	public Toggle playedBattleground;
	public Toggle playedOurGame;
	public Color highlightColor;
	public Color defaultColor;

	private void Start()
    {
		audioSource = GetComponent<AudioSource>();

		BlinkInputField();
		PlayerInfoUpdate();
	}

	void PlayerInfoUpdate()
	{
		PlayerInfoManager.Instance.TitleUpdate();
		PlayerInfoManager.Instance.isTitled = true;
		if (PlayerInfoManager.Instance.AfterSurvey())
		{
			surveyPanelBefore.gameObject.SetActive(false);
			surveyPanelAfter.SetActive(true);
			surveyPanelAfter.GetComponent<Animator>().SetTrigger("Show");
			playText.color = defaultColor;
			surveyText.color = highlightColor;
		}

		if (PlayerInfoManager.Instance.isFinishedOurGame)
		{
			cyberneticButton.interactable = false;
			cyberneticButton.GetComponent<Animator>().enabled = false;
			cyberneticButton.image.color = buttonDisableColor;
			cyberneticButton.transform.GetChild(1).gameObject.SetActive(true);
			playedOurGame.isOn = true;

			surveyPanelBefore.gameObject.SetActive(false);

		}
		else if (PlayerInfoManager.Instance.isFinishedBattleground)
		{
			battlegroundButton.interactable = false;
			battlegroundButton.GetComponent<Animator>().enabled = false;
			battlegroundButton.image.color = buttonDisableColor;
			battlegroundButton.transform.GetChild(1).gameObject.SetActive(true);
			playedBattleground.isOn = true;

			surveyPanelBefore.gameObject.SetActive(false);
		}
	}

    public void Check()
    {
        if(nameInput.text != "" && ageInput.text != "" && isSelected) {
            toggle.interactable = true;
            ActivateToogle();
        }

        else
        {
            toggle.interactable = false;
            toggle.isOn = false;
            DeactivateToogle();
        }
    }

    void ActivateToogle()
    {
        Color _targetColor;
        _targetColor = activateColor;
        toggle.transform.GetChild(0).GetComponent<Image>().color = _targetColor;
        toggle.transform.GetChild(1).GetComponent<Text>().color = _targetColor;
    }

    void DeactivateToogle()
    {
        Color _targetColor;
        _targetColor = deactivateColor;
        toggle.transform.GetChild(0).GetComponent<Image>().color = _targetColor;
        toggle.transform.GetChild(1).GetComponent<Text>().color = _targetColor;
    }

	bool isMan;
	bool isWoman;
	bool isSelected;
	public void ManToggleValueChanged()
	{
		isSelected = true;
		isMan = !isMan;

		if (isMan) {
			man.interactable = false;
			woman.interactable = true;
			if (woman.isOn) woman.isOn = false;
		}
		else {
			man.interactable = true;
		}

		ProfileManager.Instance.Check();
	}

	public void WomanToggleValueChanged()
	{
		isSelected = true;
		isWoman = !isWoman;

		if (isWoman)
		{
			woman.interactable = false;
			man.interactable = true;
			if (man.isOn) man.isOn = false;
		}
		else
		{
			woman.interactable = true;
		}

		ProfileManager.Instance.Check();
	}

	public void ToggleValueChanged()
    {
		isAgreed = !isAgreed;
        Color _targetColor = isAgreed ? activateColor : deactivateColor;
        button.transform.GetChild(0).GetComponent<Image>().color = _targetColor;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = _targetColor;
        button.interactable = !button.interactable;
    }

	public void ManHoverEnter()
	{
		if (isRegistered || !man.interactable) return;

		man.transform.GetChild(0).GetComponent<Image>().color = hoverEnterColor;
		man.transform.GetChild(1).GetComponent<Text>().color = hoverEnterColor;
	}

	public void ManHoverExit()
	{
		if (isRegistered) return;
		man.transform.GetChild(0).GetComponent<Image>().color = deactivateColor;
		man.transform.GetChild(1).GetComponent<Text>().color = deactivateColor;
	}

	public void WomanHoverEnter()
	{
		if (isRegistered || !woman.interactable) return;

		woman.transform.GetChild(0).GetComponent<Image>().color = hoverEnterColor;
		woman.transform.GetChild(1).GetComponent<Text>().color = hoverEnterColor;
	}

	public void WomanHoverExit()
	{
		if (isRegistered) return;
		woman.transform.GetChild(0).GetComponent<Image>().color = deactivateColor;
		woman.transform.GetChild(1).GetComponent<Text>().color = deactivateColor;
	}


	public void HoverEnter()
    {
        if (isRegistered || !isAgreed) return;
        button.transform.GetChild(0).GetComponent<Image>().color = hoverEnterColor;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = hoverEnterColor;
    }

    public void HoverExit()
    {
        if (isRegistered || !isAgreed) return;
        button.transform.GetChild(0).GetComponent<Image>().color = hoverExitColor;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = hoverExitColor;
    }

	public void ToggleHoverEnter()
    {
        if (isRegistered || !toggle.interactable) return;
        toggle.transform.GetChild(0).GetComponent<Image>().color = hoverEnterColor;
        toggle.transform.GetChild(1).GetComponent<Text>().color = hoverEnterColor;
    }

    public void ToggleHoverExit()
    {
        if (isRegistered || !toggle.interactable) return;
        toggle.transform.GetChild(0).GetComponent<Image>().color = hoverExitColor;
        toggle.transform.GetChild(1).GetComponent<Text>().color = hoverExitColor;
    }

    public void ButtonClicked()
    {
		audioSource.PlayOneShot(selectClip);
		isRegistered = true;
        button.interactable = false;
        button.transform.GetChild(0).GetComponent<Image>().color = deactivateColor;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = deactivateColor;


		string sex = man.isOn ? "남성(man)" : "여성(woman)";
		PlayerInfoManager.Instance.SavePlayerData(nameInput.text, ageInput.text, sex);
        StartCoroutine(ShowSelectSceneUI());
    }

    public void BlinkInputField()
    {
        StartCoroutine(TextFader());
    }

    IEnumerator ShowSelectSceneUI()
    {
        animator.enabled = false;

        float alpha = thankyouText.color.a;

        while (alpha < 0.95f)
        {
            alpha = Mathf.Clamp(alpha += Time.deltaTime, 0, 0.95f);
            thankyouText.color = new Color(thankyouText.color.r, thankyouText.color.g, thankyouText.color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        animator.enabled = true;
        animator.SetTrigger("LoginFadeOut");

		yield return new WaitForSeconds(2f);
		if(surveyPanelBefore.gameObject.activeSelf) surveyPanelBefore.ShowSurveyPanel();
	}

    IEnumerator TextFader()
    {
        yield return new WaitForSeconds(8.5f);
        float alpha = .5f;
        while (true)
        {
            Color currentColor = nameInput.placeholder.color;
            nameInput.placeholder.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            ageInput.placeholder.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            alpha = Mathf.PingPong(Time.time, 1f);
            yield return null;
        }
    }
}
