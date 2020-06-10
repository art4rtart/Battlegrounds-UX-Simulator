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

    public bool isBattleground;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            FirstPersonController.Instance.mouseManager.UnlockCursor();
            StopAllCoroutines();
            if (isBattleground) { StartCoroutine(BlurScreen()); }
            WeaponController.Instance.weaponAnimator.SetBool("Aim", false);
            WeaponController.Instance.weaponAnimator.SetBool("Walk", false);
            WeaponController.Instance.weaponAnimator.SetBool("Run", false);
            PauseUICanvas.SetActive(!PauseUICanvas.activeSelf);
            weaponUICanvas.SetActive(!weaponUICanvas.activeSelf);
            WeaponController.Instance.enabled = !WeaponController.Instance.enabled;

            if (isPaused) FirstPersonController.Instance.LockHeadMovement();
            else FirstPersonController.Instance.UnLockHeadMovement();
        }
    }

    IEnumerator BlurScreen()
    {
        // GameTimeController.isPaused = isPaused;

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
