using UnityEngine;

public class WalkingState : IPlayerState
{
    public WalkingState (PlayerController pc) : base("WalkingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    
    private bool _textActive = false;
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
        
        if(_pc.InteractableRange){
            _textActive = true;
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput(),_pc.InteractableText);
            if (_pc.InteractPressed && _pc.KeyDebounced)
                _playerStateMachine.ChangeState(_pc.InteractingState);
        }
        if ((Mathf.Abs(_pc.Movement.x) < Mathf.Epsilon)&&(Mathf.Abs(_pc.Movement.y) < Mathf.Epsilon))
            _playerStateMachine.ChangeState(_pc.IdleState);
        if (_pc.SpacePressed && _pc.canJump)
            _playerStateMachine.ChangeState((_pc.JumpingState));
        if (_pc._isRunning)
            _playerStateMachine.ChangeState(_pc.SprintingState);
        if (_pc._canDash && _pc.dashPressed ) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        if (_pc.SwingPressed && _pc._canSwing && _pc.InRange)
            _playerStateMachine.ChangeState(_pc.SwingingState);
        if(!_pc.IsGrounded())
            _playerStateMachine.ChangeState(_pc.FallingState);
        
        if (_pc.InDialogeTriggerZone && _pc.NPC.hasBeenTalkedTo == false)
        {
            _textActive = true;
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput(), _pc.DialogueText);
            if (_pc.InteractPressed)
            {
                _playerStateMachine.ChangeState(_pc.TalkingState);
            }
        }

        if (!_pc.InteractableRange && !_pc.InDialogeTriggerZone)
        {
            DisableInteractDialogueActive(_pc.GetUIController());
        }
    }

    public override void ExitState()
    {
        //_playerStateMachine.Animator.SetBool("IsWalking", false);        
        _pc.GetAudio().Stop();
        if (_textActive)
        {
            DisableInteractDialogueActive(_pc.GetUIController());
            _textActive = false;
        }

    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * ((_pc.MoveSpeed * _pc.SpeedBoostMultiplier) * Time.deltaTime), 
            Space.World);
        _pc.FootPrint(_pc.footstepIntervalWalking);
        
    }
    
}