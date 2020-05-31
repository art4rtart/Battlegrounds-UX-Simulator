using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<PauseMenuController>();
            return instance;
        }
    }
    private static PauseMenuController instance;

    public float lerpSpeed = 20f;
    public bool isPaused;

    public GameObject PauseUICanvas;
    public GameObject weaponUICanvas;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
        {
            FirstPersonController.Instance.mouseManager.UnlockCursor();
            StopAllCoroutines();
            StartCoroutine(BlurScreen());
            PauseUICanvas.SetActive(!PauseUICanvas.activeSelf);
            weaponUICanvas.SetActive(!weaponUICanvas.activeSelf);
            FirstPersonController.Instance.enabled = !FirstPersonController.Instance.enabled;
        }
    }

    IEnumerator BlurScreen()
    {
        isPaused = !isPaused;

        GameTimeController.isPaused = isPaused;

        float iteration = BlurEffect.Instance.iterations;
        float lerpValue = 0;

        if (isPaused)
        {
            BlurEffect.Instance.downRes = 1;

            while (iteration < 50)
            {
                iteration = Mathf.Lerp(0, 50, lerpValue);
                lerpValue += Time.deltaTime * lerpSpeed;

                BlurEffect.Instance.iterations = (int)iteration;
                yield return null;
            }
        }

        else
        {
            while (iteration > 0)
            {
                iteration = Mathf.Lerp(50, 0, lerpValue);
                lerpValue += Time.deltaTime * lerpSpeed;

                BlurEffect.Instance.iterations = (int)iteration;
                yield return null;
            }

            BlurEffect.Instance.downRes = 0;
        }
    }
}
