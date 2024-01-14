using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : PlayerState
{
    
    public TransitionState (PlayerController pc) : base("TransitionState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    public override void EnterState()
    {
        base.EnterState();  
        PlayerStateMachine.Animator.Play("Transition");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (PC.isTransitioning == false)  
            PlayerStateMachine.ChangeState(PC.IdleState);
    }

}
