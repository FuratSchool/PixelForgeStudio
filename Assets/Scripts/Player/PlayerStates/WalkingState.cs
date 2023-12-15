using UnityEngine;

public class WalkingState : IPlayerState
{
    public WalkingState (PlayerController pc) : base("WalkingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    
    private bool _dialogueActive = false;
    public override void EnterState()
    {
        
        
        
        _pc.GetAudio().clip = _pc.WalkingSound;
        _pc.GetAudio().Play();
        
        //_pc.MoveSpeed = _pc.WalkSpeed;
        //_playerStateMachine.Animator.Play("Walking");
        _playerStateMachine.Animator.SetInteger("State", 1);

    }
    public override void UpdateState()
    {
        base.UpdateState();
        
        if ((Mathf.Abs(_pc.Movement.x) < Mathf.Epsilon)&&(Mathf.Abs(_pc.Movement.y) < Mathf.Epsilon))
            _playerStateMachine.ChangeState(_pc.IdleState);
        if (_pc.SpacePressed && _pc.canJump)
            _playerStateMachine.ChangeState((_pc.JumpingState));
        if (_pc._isRunning)
            _playerStateMachine.ChangeState(_pc.SprintingState);
        if (_pc._canDash && _pc.dashPressed) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        if (_pc.SwingPressed && _pc._canSwing && _pc.InRange)
            _playerStateMachine.ChangeState(_pc.SwingingState);
        if(!_pc.IsGrounded())
            _playerStateMachine.ChangeState(_pc.FallingState);
        
        if (_pc.InDialogeTriggerZone && _pc.NPC.hasBeenTalkedTo == false)
        {
            _dialogueActive = true;
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput());
            if (_pc.InteractPressed)
            {
                _playerStateMachine.ChangeState(_pc.TalkingState);
            }
        }
        else
        {
            if (_dialogueActive)
            {
                DisableInteractDialogueActive(_pc.GetUIController());
                _dialogueActive = false;
            }
        }
    }

    public override void ExitState()
    {
        //_playerStateMachine.Animator.SetBool("IsWalking", false);        
        _pc.GetAudio().Stop();
        if (_dialogueActive)
        {
            DisableInteractDialogueActive(_pc.GetUIController());
            _dialogueActive = false;
        }

    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);
        _pc.FootPrint(_pc.footstepIntervalWalking);
        
    }
    
}