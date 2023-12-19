using UnityEngine;

public class IdleState : IPlayerState
{
    public IdleState (PlayerController pc) : base("IdleState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    
    private bool _textActive = false;
    public override void EnterState()
    {
        base.EnterState();
        _playerStateMachine.Animator.Play("Idle");
        _playerStateMachine.Animator.SetInteger("State", 0);

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
        if ((Mathf.Abs(_pc.Movement.x) > Mathf.Epsilon)||(Mathf.Abs(_pc.Movement.y) > Mathf.Epsilon))
            _playerStateMachine.ChangeState(_pc.WalkingState);
        if (_pc.SpacePressed && _pc.canJump)
            _playerStateMachine.ChangeState((_pc.JumpingState));
        if (_pc._canDash && _pc.dashPressed && _pc.KeyDebounced) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        if (_pc.SwingPressed && _pc._canSwing && _pc.InRange)
            _playerStateMachine.ChangeState(_pc.SwingingState);
        if(!_pc.IsGrounded())
            _playerStateMachine.ChangeState(_pc.FallingState);

        
        if (_pc.InDialogeTriggerZone && _pc.NPC.hasBeenTalkedTo == false)
        {
            _textActive = true;
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput(),_pc.DialogueText);
            if (_pc.InteractPressed)
            {
                _playerStateMachine.ChangeState(_pc.TalkingState);
            }
        }
    }
    public override void ExitState()
    {
        if (_textActive)
        {
            DisableInteractDialogueActive(_pc.GetUIController());
            _textActive = false;
        }
    }
}