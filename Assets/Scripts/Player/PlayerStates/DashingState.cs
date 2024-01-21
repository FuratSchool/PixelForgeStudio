using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashingState : PlayerState
{
    private AudioSource dashSound;
    public DashingState (PlayerController pc) : base("DashingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    public override void EnterState()
    {
        if(Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(PC.MotorRumbleLowFreq,PC.MotorRumbleHighFreq);
        dashSound= PC.JumpingSound;
        dashSound.pitch = 0.6f;
        dashSound.volume = 0.2f;
        dashSound.Play();
        PlayerStateMachine.Animator.SetInteger("State", 4);
        PC.StartCoroutine(PC.Dash());
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if (!PC.IsGrounded() && !PC.isDashing)
        {
            PC.dashPressed = false;
            PlayerStateMachine.ChangeState((PC.FallingState));
        }

        else if (PC.IsGrounded() && !PC.isDashing)
        {
            PC.dashPressed = false;
            PlayerStateMachine.ChangeState((PC.IdleState));
        }

    }

    public override void ExitState()
    {
        if(Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0,0);
        dashSound.pitch = 1f;
        dashSound.volume = 1f;
    }
}