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

	public bool isPlayedBattleground;
	public bool isPlayedOurGame;

	public void SavePlayerSurveyData(string _playerHasPlayed, string _playerGames, string _playerSkilled)
	{
		playerHasPlayed = _playerHasPlayed;
		playerGames = _playerGames;
		playerSkilled = _playerSkilled;
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
		Debug.Log(scene.name);
		if (scene.name == "Level3")
			isPlayedOurGame = true;
	}
}
