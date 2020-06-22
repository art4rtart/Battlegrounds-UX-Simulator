using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
	public static SceneMaster Instance { get; private set; }

	Animator animator;
    GameObject LoadingObject;
    public string sceneName;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        LoadingObject = this.transform.GetChild(0).gameObject;
    }

    public void LoadLevel1()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator.SetTrigger("Load");
        sceneName = "Battleground";

    }

    public void LoadLevel2()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		animator.SetTrigger("Load");
        sceneName = "Level1";
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("OurGame");
    }

	public void LoadCredit()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		animator.SetTrigger("Load");
		sceneName = "Credit";
	}
}
