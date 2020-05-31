using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public void Awake()
    {
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
        SceneManager.LoadScene("Battleground");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("LAB25");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Survey");
    }
}
