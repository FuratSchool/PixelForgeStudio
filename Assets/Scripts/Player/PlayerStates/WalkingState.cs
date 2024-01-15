using UnityEngine;

public class WalkingState : PlayerState
{
    public WalkingState (PlayerController pc) : base("WalkingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    private bool _textActive = false;
    public override void EnterState()
    {
        PC.GetAudio().clip = PC.WalkingSound;
        PC.GetAudio().Play();
        PlayerStateMachine.Animator.SetInteger("State", 1);
    }
    public override void UpdateState()
    {
        base.UpdateState();
        
        if (PC.InDialogeTriggerZone && PC.NPC.hasBeenTalkedTo == false)
        {
            _textActive = true;
            EnableInteractDialogueActive(PC.GetUIController(), PC.GetPlayerInput(), PC.DialogueText);
            if (PC.InteractPressed)
            {
                PlayerStateMachine.ChangeState(PC.TalkingState);
                return;
            }
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
        if ((Mathf.Abs(PC.Movement.x) < Mathf.Epsilon)&&(Mathf.Abs(PC.Movement.y) < Mathf.Epsilon))
            PlayerStateMachine.ChangeState(PC.IdleState);
        else if (PC.SpacePressed && PC.canJump)
            PlayerStateMachine.ChangeState((PC.JumpingState));
        else if (PC.isRunning)
            PlayerStateMachine.ChangeState(PC.SprintingState);
        else if (PC._canDash && PC.dashPressed ) 
            PlayerStateMachine.ChangeState(PC.DashingState);
        else if (PC.SwingPressed && PC._canSwing && PC.InRange)
            PlayerStateMachine.ChangeState(PC.SwingingState);
        else if(!PC.IsGrounded())
            PlayerStateMachine.ChangeState(PC.FallingState);
        
        

        else if (!PC.InteractableRange && !PC.InDialogeTriggerZone)
        {
            DisableInteractDialogueActive(PC.GetUIController());
        }
    }

    public override void ExitState()
    {    
        PC.GetAudio().Stop();
        if (_textActive)
        {
            DisableInteractDialogueActive(PC.GetUIController());
            _textActive = false;
        }
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
            Space.World);
        PC.FootPrint(PC.footstepIntervalWalking);
    }
}