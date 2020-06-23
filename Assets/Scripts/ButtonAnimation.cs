using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonAnimation : MonoBehaviour
{
    Animator animator;
	public SceneMaster sceneMaster;

    private void Awake()
    {
        animator = GetComponent<Animator>();
		sceneMaster = FindObjectOfType<SceneMaster>();

	}

    public void HoverEnter()
    {
        animator.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        animator.SetBool("Hover", false);
    }

	public string sceneName;
	public void OnClickButton()
	{
		Debug.Log(sceneMaster);
		sceneMaster.LoadLevel(sceneName);
	}
}
