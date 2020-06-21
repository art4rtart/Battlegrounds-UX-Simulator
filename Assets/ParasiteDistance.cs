using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParasiteDistance : MonoBehaviour
{
	ParasiteController parasiteController;
	public float distance = 30f;

	private void Awake()
	{
		parasiteController = GetComponent<ParasiteController>();
	}

	private void Update()
	{
		if (Vector3.Distance(this.transform.position, FirstPersonController.Instance.gameObject.transform.position) < distance && !parasiteController.foundTarget)
		{
			parasiteController.agent.speed = 1.5f;
			parasiteController.animator.SetInteger("ZombieType", 1);
			parasiteController.foundTarget = true;
		}
	}
}
