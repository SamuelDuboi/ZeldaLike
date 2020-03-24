using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class WindSlash : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (Mathf.Abs( Input.GetAxis("Horizontal")) > 0.6f)
           SD_PlayerMovement.Instance.XAxis = Input.GetAxisRaw("Horizontal");
       
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.6f)
            SD_PlayerMovement.Instance.YAxis = Input.GetAxisRaw("Vertical");


        SD_PlayerMovement.Instance.cantMove = true;
        SD_PlayerMovement.Instance.Move();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }


}
