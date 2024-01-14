using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerController pc) : base("JumpingState", pc)
    {
        PC = (PlayerController)this.PlayerStateMachine;
    }
    
    private bool _uiActive = false;
    public override void EnterState()
    {
        base.EnterState();
        PC.jumped = true;
        PC.isJumping = true;
        PC.GetAudio().PlayOneShot(PC.JumpingSound);
        PC.EnableGrimParticles(false);
        PlayerStateMachine.Animator.SetInteger("State", 3);
        StartJump();

    }

    public override void UpdateState()
    {
        
        base.UpdateState();
        if (PC._canDash && PC.dashPressed) 
            PlayerStateMachine.ChangeState(PC.DashingState);
        else if (PC.SpacePressed && PC.canDoubleJump && PC.jumpReleased)
            PlayerStateMachine.ChangeState(PC.DoubleJumpState);


        if (CheckSwing())
        {
            _uiActive = true;
            EnableSwingText(PC.GetUIController(), PC.GetPlayerInput());
            if (PC.SwingPressed)
            {
                PlayerStateMachine.ChangeState(PC.SwingingState);
                return;
            }
        }
        else
        {
            if(_uiActive)
            {
                DisableSwingText(PC.GetUIController());
                _uiActive = false;
            }
        }
    }
    
    public override void ExitState()
    {
        PC.EnableGrimParticles(true);
        PC.canJump = false;
    }
    public override void LateUpdateState()
    {
        base.LateUpdateState();
        PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
            Space.World);
        if (!PC.canJump) return; //for dialogue
        
        ContinueJump();
        
        if (!PC.SpacePressed)
        {
            PC.CanJumpAgain = true;
        }
        else
        {
            PC.CanJumpAgain = false;
        }
    }
    private void StartJump()
    {
        PC.jumpTimeCounter = PC.jumpTime;
        PC.GetRigidbody().AddForce(Vector3.up * PC.force, ForceMode.Impulse);
    }
    
    private void ContinueJump()
    {
        if (PC.jumpTimeCounter > 0 && PC.SpacePressed)
        {
            PC.GetRigidbody().AddForce(Vector3.up * PC.forceHoldJump, ForceMode.Impulse);
            PC.jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            PC.canJump = false;
            PlayerStateMachine.ChangeState(PC.FallingState);
        }
    }
}