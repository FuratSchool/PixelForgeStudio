using UnityEngine;

public class IdleState : IPlayerState
{
    public IdleState (PlayerController pc) : base("IdleState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Idle");
        _playerStateMachine.Animator.Play("Idle");

    }

    public override void UpdateState()
    {
        base.UpdateState();
       
        if ((Mathf.Abs(_pc.Movement.x) > Mathf.Epsilon)||(Mathf.Abs(_pc.Movement.y) > Mathf.Epsilon))
            _playerStateMachine.ChangeState(_pc.WalkingState);
        if (_pc.SpacePressed && _pc.canJump)
            _playerStateMachine.ChangeState((_pc.JumpingState));
        if (_pc._canDash && _pc.dashPressed) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        if (_pc.SwingPressed && _pc._canSwing && _pc.InRange)
            _playerStateMachine.ChangeState(_pc.SwingingState);
        if(!_pc.IsGrounded())
            _playerStateMachine.ChangeState(_pc.FallingState);

        
        if (_pc.InDialogeTriggerZone && _pc.NPC.hasBeenTalkedTo == false)
        {
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput());
            if (_pc.InteractPressed)
            {
                _playerStateMachine.ChangeState(_pc.TalkingState);
            }
        }
        else
        {
            DisableInteractDialogueActive(_pc.GetUIController());
        }
    }
    public override void ExitState()
    {
        DisableInteractDialogueActive(_pc.GetUIController());
        //_playerStateMachine.Animator.enabled = false;

    }
}