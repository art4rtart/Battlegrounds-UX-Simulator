using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilSceneController : MonoBehaviour
{
	public string sceneName;

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SkipThisScene();
		}
    }

	void SkipThisScene()
	{
		SceneController.Instance.waitTime = 0f;
		SceneController.Instance.LoadScene(sceneName);
	}
}
