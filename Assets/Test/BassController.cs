using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BassController : MonoBehaviour
{
    public Animator animator;
    AudioSource audioSource;

    public float[] _samples = new float[512];
    public float[] _freqBand = new float[8];

    public float bassValue = 1f;
    bool smoothBass = false;
    public float waitForSeconds = .5f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ValueChanger());
    }

    IEnumerator ValueChanger()
    {
        while (true)
        {
            smoothBass = false;
            yield return new WaitForSeconds(waitForSeconds);
        }
    }

    public void Update()
    {
        if (_freqBand[4] > bassValue && !smoothBass)
        {
            smoothBass = true;
            CameraShakeController.Instance.CameraShake();
            animator.SetTrigger("Bass");
        }

        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);

        int count = 0;
        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }
    }
}