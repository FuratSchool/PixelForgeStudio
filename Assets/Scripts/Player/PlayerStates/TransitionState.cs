using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : IPlayerState
{
    
    public TransitionState (PlayerController pc) : base("TransitionState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        _playerStateMachine.Animator.SetBool("IsTransition", true);        
    }

    public override void UpdateState()
    {
        if (_pc.isTransitioning == false)  
            _playerStateMachine.ChangeState(_pc.IdleState);
    }

    public override void FixedUpdateState()
    {
    }

    public override void LateUpdateState()
    {
    }

    public override void ExitState()
    {
        _playerStateMachine.Animator.SetBool("IsTransition", false);        
    }
}
