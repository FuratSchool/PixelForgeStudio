using UnityEngine;

public class TalkingState : IPlayerState
{
    public TalkingState (PlayerController pc) : base("TalkingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        _pc.NPC.hasBeenTalkedTo = true;
        _pc.GetDialogueManager().StartDialogue(_pc.NPC.dialogue);
        _pc.DialogueActive = true;
        _playerStateMachine.Animator.Play("Idle");
    }

    public override void UpdateState()
    {
        if (!_pc.DialogueActive)
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
        _pc.DisableInteractDialogueActive(_pc.GetUIController());
        _pc.NPC.hasBeenTalkedTo = true;
        _pc.SpacePressed = false;
        if(_pc.NPC.canTalkAgain)
            _pc.NPC.hasBeenTalkedTo = false;
        //_playerStateMachine.Animator.SetBool(("IsTalking"), false);

    }
    
    
    
}