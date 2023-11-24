using UnityEngine;

public class SprintingState : IPlayerState
{
    public SprintingState (PlayerController pc) : base("SprintingState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    public override void EnterState()
    {
        base.EnterState();
        _pc.MoveSpeed = _pc.SprintSpeed;
        _playerStateMachine.Animator.Play("Sprinting");

    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (Mathf.Abs(_pc.Movement.x) < Mathf.Epsilon && Mathf.Abs(_pc.Movement.y) < Mathf.Epsilon)
            _playerStateMachine.ChangeState(_pc.IdleState);
        if (_pc.SpacePressed && _pc.canJump)
            _playerStateMachine.ChangeState((_pc.JumpingState));
        if (!_pc._isRunning)
            _playerStateMachine.ChangeState(_pc.WalkingState);
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
        _pc.MoveSpeed = _pc.WalkSpeed;
        //_playerStateMachine.Animator.SetBool("IsSprinting", false);        
        _pc.GetAudio().Stop();
        DisableInteractDialogueActive(_pc.GetUIController());
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);
        _pc.FootPrint(_pc.footstepIntervalRunning);
    }
}