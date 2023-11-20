using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : IPlayerState
{
    
    public TransitionState (PlayerController pc) : base("TransitionState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        base.EnterState();  
        _playerStateMachine.Animator.Play("Idle");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_pc.isTransitioning == false)  
            _playerStateMachine.ChangeState(_pc.IdleState);
    }
}
