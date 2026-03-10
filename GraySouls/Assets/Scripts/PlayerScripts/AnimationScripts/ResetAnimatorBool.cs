using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.SetBool("isInteracting", false);
    //}
    public string targetBool;
    public string targetFloat;
    public bool status;
    public float number;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
        if (targetFloat!="")
        {
            animator.SetFloat(targetFloat, number);
        }
    }
}
