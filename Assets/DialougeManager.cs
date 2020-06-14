using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialougeManager : MonoBehaviour
{

    [TextArea(0,100)]
    public string[] sentencesENG;

    [TextArea(0, 100)]
    public string[] sentencesKOR;

    Animator animator;
    AudioSource audioSource;

    [Header("Clip Sounds")]
    public AudioClip[] audioClip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LockPlayerMovement()
    {
        FirstPersonController.Instance.enabled = false;
        WeaponController.Instance.enabled = false;
    }

    public void UnlockPlayerMovement()
    {
        FirstPersonController.Instance.enabled = true;
        WeaponController.Instance.enabled = true;
    }

    public void FadeInSound()
    {
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }

    public void FadeOutSound()
    {
        audioSource.clip = audioClip[1];
        audioSource.Play();
    }

    public CanvasGroup messageCanvasGroup;

    public void ShowTutorialMessage()
    {
        StartCoroutine(ShowMessage());
    }

    public float messageFadeSpeed = 2f;
    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(.5f);
        float value = 0;

        FadeInSound();
        while(value < 1)
        {
            value += Time.deltaTime * messageFadeSpeed;
            messageCanvasGroup.alpha = value;
            yield return null;
        }

        value = 1.25f;

        yield return new WaitForSeconds(3f);
        FadeOutSound();
        while (value > 0)
        {
            value -= Time.deltaTime * messageFadeSpeed;
            messageCanvasGroup.alpha = value;
            yield return null;
        }

        StartCoroutine(ShowTime());
    }


    public float waitSecondsBeforeSceneChange = 5f;
    public CanvasGroup timeCanvasGroup;
    public float totalWaitTime = 10f;

    public TextMeshProUGUI timeText;

    IEnumerator ShowTime()
    {
        yield return new WaitForSeconds(waitSecondsBeforeSceneChange);

        float value = 0;

        FadeInSound();
        while (value < 1)
        {
            value += Time.deltaTime * messageFadeSpeed;
            timeCanvasGroup.alpha = value;
            yield return null;
        }

        while(totalWaitTime > 0)
        {
            totalWaitTime = Mathf.Clamp(totalWaitTime -= Time.deltaTime, 0f, totalWaitTime);
            timeText.text = totalWaitTime.ToString("N2");
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        FadeOutSound();
        while (value > 0)
        {
            value -= Time.deltaTime * messageFadeSpeed;
            timeCanvasGroup.alpha = value;
            yield return null;
        }

        animator.SetTrigger("FadeOut");
    }

    public void MoveToNextScene()
    {
        SceneController.Instance.LoadScene("Level2");
    }
}
