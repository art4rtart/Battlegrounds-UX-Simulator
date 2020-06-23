using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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

	public bool isFinishedBattleground;
	public bool isFinishedOurGame;

	public bool isTitled;

	[Header("Player Info")]
	public string playerName;       // 이름
	public string playerAge;        // 나이
	public string playerSex;        // 성별

	[Header("Player Survey  Info")]
	public string playerHasPlayed;  // 플레이 해봤는지
	public string playerGames;      // 플레이 해본 게임
	public string playerSkilled;        // 숙련도

	[Header("Player Survey  Info")]
	public string battleGroundLevel;  
	public string ourGameLevel;      
	public string verseLevel;        
	public string battleGroundPerfer;  
	public string ourGamePerfer;      
	public string versePerfer;       
	public string intuition;  
	public string aesthetic;     
	public string efficiency;      
	public string feedback;  

	[Header("Our Game Level2")]
	public float level2TotalPlayTime;
	public float level2TotalTabTime;
	public float[] level2CustomTime = new float[3];

	[Header("Our Game Level3")]
	public float level3TotalPlayTime;
	public float level3TotalTabTime;
	public float[] level3CustomTime = new float[3];

	[Header("Battleground Level2")]
	public float level2BGTotalPlayTime;
	public float level2BGTotalTabTime;
	public float[] level2BGCustomTime = new float[3];

	[Header("Battleground Level3")]
	public float level3BGTotalPlayTime;
	public float level3BGTotalTabTime;
	public float[] level3BGCustomTime = new float[3];

	string source = ""; //읽어낸 텍스트 할당받는 변수
	public string filePath = "";

	public float mouseSensitivity;

	public void SavePlayerSurveyData(string _playerHasPlayed, string _playerGames, string _playerSkilled)
	{
		playerHasPlayed = _playerHasPlayed;
		playerGames = _playerGames;
		playerSkilled = _playerSkilled;
	}

	public void SavePlayerSurveyDataAfterGame(string _battleGroundLevel, string _ourGameLevel, string _verseLevel, 
		string _battleGroundPerfer, string _ourGamePerfer, string _versePerfer,
		string _intuition, string _aesthetic, string _efficiency, string _feedback)
	{
		battleGroundLevel = _battleGroundLevel;
		ourGameLevel = _ourGameLevel;
		verseLevel = _verseLevel;
		battleGroundPerfer = _battleGroundPerfer;
		ourGamePerfer = _ourGamePerfer;
		versePerfer = _versePerfer;
		intuition = _intuition;
		aesthetic = _aesthetic;
		efficiency = _efficiency;
		feedback = _feedback;
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

		if (scene.name == "ReworkedLevel2")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level2CustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level2TotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level2TotalTabTime = TimeMeasureController.Instance.customizationTime;
		}
		else if (scene.name == "ReworkedLevel3")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level3CustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level3TotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level3TotalTabTime = TimeMeasureController.Instance.customizationTime;
		}

		else if (scene.name == "ReworkedBGLevel2BG")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level2BGCustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level2BGTotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level2BGTotalTabTime = TimeMeasureController.Instance.customizationTime;
		}
		else if (scene.name == "ReworkedBGLevel3BG")
		{
			for (int i = 0; i < TimeMeasureController.Instance.customTime.Length; i++)
			{
				level3BGCustomTime[i] = TimeMeasureController.Instance.customTime[i];
			}
			level3BGTotalPlayTime = TimeMeasureController.Instance.totalGameTime;
			level3BGTotalTabTime = TimeMeasureController.Instance.customizationTime;
		}

		if (scene.name == "ReworkedLevel3BG") isFinishedBattleground = true;
		else if (scene.name == "ReworkedLevel3") isFinishedOurGame = true;
	}

	public bool AfterSurvey()
	{
		bool value = isFinishedBattleground && isFinishedOurGame? true: false;

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

	public void SaveData()
	{
		filePath = Application.dataPath + "/StreamingAssets" + "/" + playerName + "(" + playerAge + "), " + playerSex + ".txt";
		// FileMode.Create는 덮어쓰기.
		FileStream f = new FileStream(filePath, FileMode.Create, FileAccess.Write);

		StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);

		writer.WriteLine(playerName + "(" + playerAge + "), " + playerSex);

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("배틀로얄 게임 " + playerHasPlayed);
		writer.WriteLine("플레이 해본 게임 : " + playerGames);
		writer.WriteLine("숙련도 : " + playerSkilled);

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("배틀그라운드 Level 2");
		writer.WriteLine("커스텀 시간 (1): " + level2BGCustomTime[0].ToString("N2"));
		writer.WriteLine("커스텀 시간 (2): " + level2BGCustomTime[1].ToString("N2"));
		writer.WriteLine("커스텀 시간 (3): " + level2BGCustomTime[2].ToString("N2"));
		writer.WriteLine("총 TAB 시간: " + level2BGTotalTabTime.ToString("N2"));
		writer.WriteLine("총 플레이 시간: " + level2BGTotalPlayTime.ToString("N2"));
		writer.WriteLine("\n");
		writer.WriteLine("배틀그라운드 Level 3");
		writer.WriteLine("커스텀 시간 (1): " + level3BGCustomTime[0].ToString("N2"));
		writer.WriteLine("커스텀 시간 (2): " + level3BGCustomTime[1].ToString("N2"));
		writer.WriteLine("커스텀 시간 (3): " + level3BGCustomTime[2].ToString("N2"));
		writer.WriteLine("총 TAB 시간: " + level3BGTotalTabTime.ToString("N2"));
		writer.WriteLine("총 플레이 시간: " + level3BGTotalPlayTime.ToString("N2"));

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("새로운UI Level 2");
		writer.WriteLine("커스텀 시간 (1): " + level2CustomTime[0].ToString("N2"));
		writer.WriteLine("커스텀 시간 (2): " + level2CustomTime[1].ToString("N2"));
		writer.WriteLine("커스텀 시간 (3): " + level2CustomTime[2].ToString("N2"));
		writer.WriteLine("총 TAB 시간: " + level2TotalTabTime.ToString("N2"));
		writer.WriteLine("총 플레이 시간: " + level2TotalPlayTime.ToString("N2"));
		writer.WriteLine("\n");
		writer.WriteLine("새로운UI Level 3");
		writer.WriteLine("커스텀 시간 (1): " + level3CustomTime[0].ToString("N2"));
		writer.WriteLine("커스텀 시간 (2): " + level3CustomTime[1].ToString("N2"));
		writer.WriteLine("커스텀 시간 (3): " + level3CustomTime[2].ToString("N2"));
		writer.WriteLine("총 TAB 시간: " + level3TotalTabTime.ToString("N2"));
		writer.WriteLine("총 플레이 시간: " + level3TotalPlayTime.ToString("N2"));

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("학습 용이성 평가");
		writer.WriteLine("배그: " + battleGroundLevel);
		writer.WriteLine("새로운 UI: " + ourGameLevel);
		writer.WriteLine("배그 vs 새로운 UI : " + verseLevel);

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("선호도 조사");
		writer.WriteLine("배그: " + battleGroundPerfer);
		writer.WriteLine("새로운 UI: " + ourGamePerfer);
		writer.WriteLine("배그 vs 새로운 UI: " + versePerfer);

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("UI 중요도 조사");
		writer.WriteLine("직관성: " + intuition);
		writer.WriteLine("심미성: " + aesthetic);
		writer.WriteLine("효율성: " + efficiency);

		writer.WriteLine("\n---------------------------------------------\n");

		writer.WriteLine("[유저 피드백]");
		writer.WriteLine(feedback);
		writer.Close();
	}

	public void SaveDataSendMail()
	{
		// save whole data
		SaveData();
		// send email
		SendEmail.Instance.OnButtonClick();
	}

	//private void Start()
	//{
	//	SaveDataSendMail();
	//}
}
