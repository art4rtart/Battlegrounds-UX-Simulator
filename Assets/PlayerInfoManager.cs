using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoManager : MonoBehaviour
{
	public static PlayerInfoManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	[Header("Player Info")]
	public string playerName;       // 이름
	public string playerAge;        // 나이
	public string playerSex;        // 성별

	[Header("Player Survey  Info")]
	public string playerHasPlayed;  // 플레이 해봤는지
	public string playerGames;      // 플레이 해본 게임
	public string playerSkilled;        // 숙련도

	public bool isFinishedBattleground;
	public bool isFinishedOurGame;

	public bool isTitled;

	[Header("Our Game Level2")]
	public float level2TotalPlayTime;
	public float level2TotalTabTime;
	public float[] level2CustomTime = new float[3];

	[Header("Our Game Level3")]
	public float level3TotalPlayTime;
	public float level3TotalTabTime;
	public float[] level3CustomTime = new float[3];

	public void SavePlayerSurveyData(string _playerHasPlayed, string _playerGames, string _playerSkilled)
	{
		playerHasPlayed = _playerHasPlayed;
		playerGames = _playerGames;
		playerSkilled = _playerSkilled;
	}

	public void SavePlayerSurveyDataAfterGame()
	{

	}

	public void SavePlayerData(string _playerName, string _playerAge, string _playerSex)
	{
		playerName = _playerName;
		playerAge = _playerAge;
		playerSex = _playerSex;
	}

	public void SceneCompleted()
	{
		Scene scene = SceneManager.GetActiveScene();

		if (scene.name == "Level2")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level2CustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level2TotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level2TotalTabTime = TimeMeasureController.Instance.customizationTime;
		}
		if (scene.name == "Level3")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level3CustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level3TotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level3TotalTabTime = TimeMeasureController.Instance.customizationTime;
		}

		if (scene.name == "Level3") isFinishedBattleground = true;
		else if (scene.name == "BGLevel3") isFinishedOurGame = true;
	}

	public bool AfterSurvey()
	{
		bool value = isFinishedBattleground || isFinishedOurGame? true: false;

		return value;
	}

	public void TitleUpdate()
	{
		if (!isTitled) return;
		CursorController cursorController = FindObjectOfType<CursorController>();
		Animator animator = cursorController.gameObject.GetComponent<Animator>();
		ProfileManager.Instance.surveyPanelBefore.gameObject.SetActive(false);
		animator.Play("PipelineFadeInAnim");
		cursorController.ShowCursor();
	}
}
