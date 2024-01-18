using System.Collections;
using UnityEngine;

public class DashingState : PlayerState
{
    public DashingState (PlayerController pc) : base("DashingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    public override void EnterState()
    {
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
    
    public override void ExitState() { }
}