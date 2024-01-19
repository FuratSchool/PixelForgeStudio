using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState (PlayerController pc) : base("IdleState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    private bool _textActive = false;
    public override void EnterState()
    {
        base.EnterState();
        PlayerStateMachine.Animator.Play("Idle");
        PlayerStateMachine.Animator.SetInteger("State", 0);
        PC.EnableGrimParticles(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (PC.InDialogeTriggerZone && PC.NPC.hasBeenTalkedTo == false)
        {
            _textActive = true;
            EnableInteractDialogueActive(PC.GetUIController(), PC.GetPlayerInput(),PC.DialogueText);
            if (PC.InteractPressed)
            {
                PlayerStateMachine.ChangeState(PC.TalkingState);
                return;
            }
        }

        if (PC.autoTrigger)
        {
            PlayerStateMachine.ChangeState(PC.TalkingState);
            PC.autoTrigger = false;
            return;
        }
        if(PC.InteractableRange){
            _textActive = true;
            EnableInteractDialogueActive(PC.GetUIController(), PC.GetPlayerInput(),PC.InteractableText);
            if (PC.InteractPressed && PC.KeyDebounced)
            {
                PlayerStateMachine.ChangeState(PC.InteractingState);
                return;
            }
        }

        if (PC.EmotePressed)
        {
            PC.EmoteNumber = 20;
            PlayerStateMachine.ChangeState(PC.EmoteState);
        }
        else if (PC.EmoteGangPressed)
        {
            PC.EmoteNumber = 21;
            PlayerStateMachine.ChangeState(PC.EmoteState);
        }
        else if ((Mathf.Abs(PC.Movement.x) > Mathf.Epsilon)||(Mathf.Abs(PC.Movement.y) > Mathf.Epsilon))
            PlayerStateMachine.ChangeState(PC.WalkingState);
        else if (PC.SpacePressed && PC.canJump && PC.CanJumpAgain)
            PlayerStateMachine.ChangeState((PC.JumpingState));
        else if (PC._canDash && PC.dashPressed && PC.KeyDebounced) 
            PlayerStateMachine.ChangeState(PC.DashingState);
        else if (PC.SwingPressed && PC._canSwing && PC.InRange)
            PlayerStateMachine.ChangeState(PC.SwingingState);
        else if (!PC.IsGrounded())
        {
            PlayerStateMachine.ChangeState(PC.FallingState);
        }
    }
    public override void ExitState()
    {
        PC.EnableGrimParticles(true);
        if (_textActive)
        {
            DisableInteractDialogueActive(PC.GetUIController());
            _textActive = false;
        }
    }
}