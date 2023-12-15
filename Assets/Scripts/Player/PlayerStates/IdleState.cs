using UnityEngine;

public class IdleState : IPlayerState
{
    public IdleState (PlayerController pc) : base("IdleState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    
    private bool dialogueActive = false;
    public override void EnterState()
    {
        base.EnterState();
        _playerStateMachine.Animator.Play("Idle");
        _playerStateMachine.Animator.SetInteger("State", 0);

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
            dialogueActive = true;
            EnableInteractDialogueActive(_pc.GetUIController(), _pc.GetPlayerInput());
            if (_pc.InteractPressed)
            {
                _playerStateMachine.ChangeState(_pc.TalkingState);
            }
        }
        else
        {
            if (dialogueActive)
            {
                DisableInteractDialogueActive(_pc.GetUIController());
                dialogueActive = false;
            }
            
        }
    }
    public override void ExitState()
    {
        if (dialogueActive)
        {
            DisableInteractDialogueActive(_pc.GetUIController());
            dialogueActive = false;
        }
    }
}