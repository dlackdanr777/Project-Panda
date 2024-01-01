using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFlip : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform transform = animator.transform;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        animator.enabled = false;
    }

}
