using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttack : StateMachineBehaviour
{
    float XAxis, YAxis;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetAxis("Horizontal") > 0.6f)
            XAxis = Input.GetAxisRaw("Horizontal");
        else
            XAxis = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Vertical") > 0.6f)
            YAxis = Input.GetAxisRaw("Vertical");
        else
            YAxis = Input.GetAxis("Vertical");

        if (XAxis < 0.4 && XAxis > -0.4 && YAxis > 0.1)
        {
            animator.SetFloat("YAxis", 1f);
            animator.SetFloat("XAxis", 0f);
        }
        else if (XAxis < 0.4 && XAxis > -0.4 && YAxis < -0.1)
        {
            animator.SetFloat("YAxis", -1f);
            animator.SetFloat("XAxis", 0f);

        }
        else if (XAxis >= 0.4f)
        {
            animator.SetFloat("XAxis", 1f);
            animator.SetFloat("YAxis", 0f);
        }
        else if (XAxis <= -0.4f)
        {
            animator.SetFloat("XAxis", -1f);
            animator.SetFloat("YAxis", 0f);

        }
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
