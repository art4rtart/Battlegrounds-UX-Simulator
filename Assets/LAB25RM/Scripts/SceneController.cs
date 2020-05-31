using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<SceneController>();
            return instance;
        }
    }
    private static SceneController instance;

    [Header("Load Settings")]
    public string scenename;
    public int scenenumber;
    public float waitTime;
    public bool flag;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(flag) StartCoroutine(LoadNextScnee(scenenumber));
    }

    IEnumerator LoadNextScnee(string _scenename)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(_scenename);
        yield return null;
    }

    IEnumerator LoadNextScnee(int _scenenumber)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(_scenenumber);
        yield return null;
    }

    public void LoadScene(int _sceneIndex)
    {
        StartCoroutine(LoadNextScnee(_sceneIndex));
    }

    public void LoadScene(string _SceneName)
    {
        StartCoroutine(LoadNextScnee(_SceneName));
    }
}
