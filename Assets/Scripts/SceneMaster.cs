using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    Animator animator;
    GameObject LoadingObject;
    public string sceneName;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        LoadingObject = this.transform.GetChild(0).gameObject;

        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
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
        animator.SetTrigger("Load");
        sceneName = "OurGame";
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
