using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAnimStateInfo : StateMachineBehaviour
{
    bool isPlaying = false;
    float time = 0;

    ParticleSystem particleSystem;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        particleSystem = animator.transform.GetChild(1).GetComponent<ParasiteController>().biteBloodParticle;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        float normalizedTime = time / stateInfo.length;
        if (normalizedTime > .25f && !isPlaying) {particleSystem.Play(); isPlaying = true; }
        else if (normalizedTime >= 1f) {particleSystem.Stop(); isPlaying = false; time = 0; }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
