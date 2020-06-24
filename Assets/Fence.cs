using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
	Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void FenceOn()
	{
		animator.SetBool("FenceOn", true);
	}

	public void FenceOff()
	{
		animator.SetBool("FenceOn", false);
	}
}
