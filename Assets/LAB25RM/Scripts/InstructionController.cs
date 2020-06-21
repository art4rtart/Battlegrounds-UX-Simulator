using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionController : MonoBehaviour
{
    public static InstructionController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<InstructionController>();
            return instance;
        }
    }
    private static InstructionController instance;

    [TextArea(0, 1000)]
    public string[] sentencesENG;

    [TextArea(0, 1000)]
    public string[] sentencesKOR;

    public int sentenceIndex;

    [HideInInspector] public Animator animator;
	[HideInInspector] public AudioSource audioSource;

    public TextMeshProUGUI instructionENGText;
    public Text instructionKORText;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateInstructions();
        }
    }

    public void UpdateInstructions()
    {
        StartCoroutine(UpdateInstruction());
	}

    IEnumerator UpdateInstruction()
    {
        audioSource.Play();
        animator.SetTrigger("Update");
        yield return new WaitForSeconds(.75f);

        sentenceIndex = Mathf.Clamp(sentenceIndex+=1, 0, sentencesENG.Length - 1);
        instructionENGText.text = sentencesENG[sentenceIndex];
        instructionKORText.text = sentencesKOR[sentenceIndex];
    }
}
