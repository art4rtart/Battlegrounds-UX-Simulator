using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSound : MonoBehaviour
{
	public AudioClip[] audioClip;
	AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(PlaySoundFx());
	}

	IEnumerator PlaySoundFx()
	{
		yield return new WaitForSeconds(2.75f);

		audioSource.clip = audioClip[0];
		audioSource.Play();
	}
}
