using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<DialougeManager>();
            return instance;
        }
    }
    private static DialougeManager instance;

    [TextArea(0,100)]
    public string[] sentencesENG;

    [TextArea(0, 100)]
    public string[] sentencesKOR;

    public int sentenceIndex;

    public CanvasGroup messageCanvasGroup;
    public TextMeshProUGUI messageENGText;
    public Text messageKORText;

    [HideInInspector] public Animator animator;
    AudioSource audioSource;

    [Header("Clip Sounds")]
    public AudioClip[] audioClip;

    public float messageFadeSpeed = 2f;

    public float waitSecondsBeforeSceneChange = 5f;
    public CanvasGroup timeCanvasGroup;
    public float totalWaitTime = 10f;

    public TextMeshProUGUI timeText;

    public string levelName;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        WeaponController.Instance.animators[WeaponController.Instance.previousGunIndex].SetBool("Holster", false);
        WeaponController.Instance.weaponAudioSource.clip = WeaponController.Instance.audioClip[2];
        WeaponController.Instance.weaponAudioSource.Play();
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

    public void ShowTutorialMessage()
    {
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        messageENGText.text = sentencesENG[sentenceIndex];
        messageKORText.text = sentencesKOR[sentenceIndex];
        sentenceIndex++;
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
        SceneController.Instance.LoadScene(levelName);
    }
}
