using System.Collections;
using UnityEngine;

public class DashingState : PlayerState
{
    private AudioSource dashSound;
    public DashingState (PlayerController pc) : base("DashingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    public override void EnterState()
    {
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
        dashSound.pitch = 1f;
        dashSound.volume = 1f;
    }
}