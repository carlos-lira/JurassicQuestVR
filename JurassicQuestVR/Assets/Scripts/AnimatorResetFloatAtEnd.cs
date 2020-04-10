using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorResetFloatAtEnd : StateMachineBehaviour
{
    [SerializeField]
    private string floatVariableName;

    // OnStateExit is called when a transition ends and the state 
    //machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(floatVariableName, 0.0f);
    }
}
