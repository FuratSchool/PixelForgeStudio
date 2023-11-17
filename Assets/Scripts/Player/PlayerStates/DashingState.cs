using System.Collections;
using UnityEngine;

public class DashingState : IPlayerState
{
    public DashingState (PlayerController pc) : base("DashingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        _playerStateMachine.Animator.Play("Start Dash");
        _pc.StartCoroutine(_pc.Dash());
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (!_pc.IsGrounded() && !_pc.isDashing)
        {
            _pc.dashPressed = false;
            _playerStateMachine.ChangeState((_pc.FallingState));
        }

        if (_pc.IsGrounded() && !_pc.isDashing)
        {
            _pc.dashPressed = false;
            _playerStateMachine.ChangeState((_pc.IdleState));
        }

    }

    public override void ExitState()
    {
        //_playerStateMachine.Animator.Play("Idle");  

    }
}