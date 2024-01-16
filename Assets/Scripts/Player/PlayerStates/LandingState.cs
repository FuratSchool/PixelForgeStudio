using System.Collections;
using UnityEngine;

public class LandingState : PlayerState
{
    public LandingState (PlayerController pc) : base("LandingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    private bool _busyLanding;
    public override void EnterState()
    {
        PC.landingParticles.Stop();
        var main = PC.landingParticles.main;
        main.startColor = PC.ColorParticles;
        PC.landingParticles.Play();
        base.EnterState();
        PlayerStateMachine.Animator.SetInteger("State", 9);
        _busyLanding = true;
        PC.StartCoroutine(Landing());
        
        PC.canJump = true; PC.canDoubleJump = true; PC.jumpReleased = false;
        PC.jumped = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_busyLanding)
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
    public override void ExitState() { }
    
    public IEnumerator Landing()
    {
        yield return new WaitForSeconds(.3f);
        _busyLanding = false;
    }
    
}