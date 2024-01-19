using System.Collections;
using UnityEngine;

public class LandingState : PlayerState
{
    public LandingState (PlayerController pc) : base("LandingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    private bool _waitStarted;
    private float timePassed;
    private AudioSource landingSound;
    public override void EnterState()
    {
        landingSound= PC.LandingSound;
        landingSound.Play();
        PC.landingParticles.Stop();
        var main = PC.landingParticles.main;
        main.startColor = PC.ColorParticles;
        PC.landingParticles.Play();
        base.EnterState();
        PlayerStateMachine.Animator.SetInteger("State", 9);
        timePassed = 0;
        if (PC.ExitSwing)
        {
            PC.ExitSwing = false;
            PC.GetRigidbody().isKinematic = true;
        }

        PC.canJump = true; PC.canDoubleJump = true; PC.jumpReleased = false;
        PC.jumped = false;

    }

    public override void UpdateState()
    {
        
        timePassed += Time.deltaTime;
        base.UpdateState();
        if (timePassed > .15f)
        {
            if ((Mathf.Abs(PC.Movement.x) > Mathf.Epsilon) || (Mathf.Abs(PC.Movement.y) > Mathf.Epsilon))
            {
                if (PC.isRunning) 
                    PlayerStateMachine.ChangeState(PC.SprintingState);
                else 
                    PlayerStateMachine.ChangeState(PC.WalkingState);
            }
            else
                PlayerStateMachine.ChangeState(PC.IdleState);
        }
    }
    
    public override void LateUpdateState()
    {
        base.LateUpdateState();
        PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
            Space.World);
    }

    public override void ExitState()
    {
        PC.GetRigidbody().isKinematic = false;
    }
}