using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReloadState : StateMachineBehaviour
{
	int count = 0;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		count = 0;
		animator.speed = WeaponController.Instance.reloadSpeed;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.normalizedTime > 0f && count == 0)
		{
			WeaponController.Instance.weaponAudioSource.clip = WeaponController.Instance.reloadSound[count];
			WeaponController.Instance.weaponAudioSource.Play();
			count++;
		}
		if (stateInfo.normalizedTime > 0.41f && count == 1)
		{
			WeaponController.Instance.weaponAudioSource.clip = WeaponController.Instance.reloadSound[count];
			WeaponController.Instance.weaponAudioSource.Play();
			count++;
		}
		if (stateInfo.normalizedTime > 0.7f && count == 2)
		{
			WeaponController.Instance.weaponAudioSource.clip = WeaponController.Instance.reloadSound[count];
			WeaponController.Instance.weaponAudioSource.Play();
			count++;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.speed = 1f;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
