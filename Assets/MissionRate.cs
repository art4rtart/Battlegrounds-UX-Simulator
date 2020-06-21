using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionRate : MonoBehaviour
{
	public static MissionRate Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<MissionRate>();
			return instance;
		}
	}
	private static MissionRate instance;

	Animator animator;
	CanvasGroup canvasGroup;
	TextMeshProUGUI tmpro;

	public float missionCountTotal;
	public float missionCountCurrent;
	public float rateSpeed = 1f;

	float rate = 0f;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		canvasGroup = GetComponent<CanvasGroup>();
		tmpro = GetComponent<TextMeshProUGUI>();
		tmpro.text = "mission clear ( 0% )";
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			UpdateMissionClearRate();
		}
	}

	public void UpdateMissionClearRate()
	{
		// animate

		missionCountCurrent++;
		StopAllCoroutines();
		StartCoroutine(UpdateMission());
	}

	IEnumerator UpdateMission()
	{
		float value = 0;

		float target = (missionCountCurrent / missionCountTotal) * 100f;

		while (value < 1)
		{
			if (rate != target)
			{
				tmpro.text = "mission clear ( <color=#FF3D3D>" + rate.ToString("N0") + "%</color> )";
				rate = Mathf.Lerp(rate, target, rateSpeed);
			}

			value += Time.deltaTime * 1.5f;
			canvasGroup.alpha = value;

			yield return null;
		}
		tmpro.text = "mission clear ( <color=#FF3D3D>" + target.ToString("N0") + "%</color> )";

		yield return new WaitForSeconds(1.5f);
		animator.Play("MissionRateMove");

		while (value > 0)
		{
			value -= Time.deltaTime;
			canvasGroup.alpha = value;
			yield return null;
		}
	}
}
