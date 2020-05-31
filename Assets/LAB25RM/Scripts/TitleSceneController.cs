using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    public static TitleSceneController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<TitleSceneController>();
            return instance;
        }
    }
    private static TitleSceneController instance;

    [HideInInspector]
    public Animator animator;
    FirstPersonController fpsController;

    public Image image;
    public bool faded = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        fpsController = GetComponent<FirstPersonController>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TitleSceneTrigger()
    {
        animator.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (mousePosition.x < 50 || mousePosition.x > Screen.width - 50
            || mousePosition.y < 50 || mousePosition.y > Screen.height - 50)
        {
            FirstPersonController.Instance.mouseManager.XSensitivity = 0;
            FirstPersonController.Instance.mouseManager.YSensitivity = 0;
        }
        else
        {
            FirstPersonController.Instance.mouseManager.XSensitivity = 0.05f;
            FirstPersonController.Instance.mouseManager.YSensitivity = 0.05f;
        }
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("1");
        faded = false;
        float alpha = 0;

        while (alpha <= 1) {
            image.color = new Color(0, 0, 0, alpha);
            alpha += Time.deltaTime;
            Debug.Log(alpha);
            yield return null;
        }
        faded = true;
        Debug.Log("2");
    }

    public IEnumerator FadeIn()
    {
        faded = false;
        float alpha = 1;

        while (alpha >= 0)
        {
            alpha -= Time.deltaTime ;
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        faded = true;
    }
}
