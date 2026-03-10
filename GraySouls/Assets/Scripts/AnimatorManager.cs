using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    public bool canRotate;
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.CrossFade(targetAnim, 0.08f);
        anim.SetBool("canRotate", false);
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        //anim.Play(targetAnim);
        //anim.CrossFade(targetAnim, 0.08f);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting,bool canRotate)
    {
        anim.CrossFade(targetAnim, 0.08f);
        anim.SetBool("canRotate", canRotate);
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        //anim.Play(targetAnim);
        //anim.CrossFade(targetAnim, 0.08f);
    }
    
    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isRotatingWithRootMotion", true);
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.08f);
    }
    public virtual void TakeCriticalDamageAnimationEvent()
    {

    }
}
