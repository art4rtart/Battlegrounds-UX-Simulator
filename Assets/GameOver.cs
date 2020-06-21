using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
	public void LoadLevelAgain()
	{
		SceneController.Instance.LoadScene(SceneController.Instance.scenename);
	}
}
