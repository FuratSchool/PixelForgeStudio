using UnityEngine;
using UnityEngine.InputSystem;

public class TalkingState : PlayerState
{
    public TalkingState (PlayerController pc) : base("TalkingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    public override void EnterState()
    {
        PC.NPC.hasBeenTalkedTo = true;
        PC.GetDialogueManager().StartDialogue(PC.NPC.dialogue);
        PC.DialogueActive = true;
        PlayerStateMachine.Animator.SetInteger("State", 7);
    }

    public override void UpdateState()
    {
        if (!PC.DialogueActive)
            PlayerStateMachine.ChangeState(PC.IdleState);
    }

    public override void FixedUpdateState() { }

    public override void LateUpdateState() { }

    public override void ExitState()
    {
        DisableInteractDialogueActive(PC.GetUIController());
        PC.StartCoroutine(PC.KeyDebounce());
        PC.NPC.hasBeenTalkedTo = true;
        PC.SpacePressed = false;
        PC.NPC.autoTrigger = false;
        if(PC.NPC.canTalkAgain)
            PC.NPC.hasBeenTalkedTo = false;
    }
}