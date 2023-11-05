using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : IPlayerState
{
    public void EnterState(PlayerStateMachine stateMachine)
    {
        stateMachine.Animator.SetBool("IsTransition", true);        
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (stateMachine.GetPlayerController().isTransitioning == false)  
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }

    public void LateUpdateState(PlayerStateMachine stateMachine)
    {
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.Animator.SetBool("IsTransition", false);        
    }
}
