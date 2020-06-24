using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEventTrigger : MonoBehaviour
{
	Animator animator;
	Slider slider;
	public float wheelSensitivity = 10f;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (animator.GetBool("Hover"))
		{
			slider.value += Input.GetAxis("Mouse ScrollWheel") * wheelSensitivity;
		}
	}
	public void HoverEnter()
	{
		if (!slider) slider = GetComponent<Slider>();
		animator.SetBool("Hover", true);
	}

	public void HoverExit()
	{
		animator.SetBool("Hover", false);
	}
}
