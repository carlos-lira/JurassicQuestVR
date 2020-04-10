using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActivateBoolAtEntrance : StateMachineBehaviour
{
    [SerializeField]
    private string booleanVariableName;

    // OnStateExit is called when a transition ends and the state 
    //machine finishes evaluating this state

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(booleanVariableName, true);
    }

}
