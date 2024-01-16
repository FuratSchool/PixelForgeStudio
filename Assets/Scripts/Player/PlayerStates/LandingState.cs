using System.Collections;
using UnityEngine;

public class LandingState : PlayerState
{
    public LandingState (PlayerController pc) : base("LandingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    private bool _waitStarted;
    public override void EnterState()
    {
        base.EnterState();
        PlayerStateMachine.Animator.SetInteger("State", 9);
        if(!_waitStarted)
            PC.StartCoroutine(Landing());
        PC.GetRigidbody().isKinematic = true;
        PC.canJump = true; PC.canDoubleJump = true; PC.jumpReleased = false;
        PC.jumped = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (PC._busyLanding)
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
    
    public IEnumerator Landing()
    {
        _waitStarted = true;
        PC._busyLanding = true;
        yield return new WaitForSeconds(.3f);
        PC._busyLanding = false;
        _waitStarted = false;
    }
    
}