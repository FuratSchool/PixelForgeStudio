using System.Collections;
using UnityEngine;

public class LandingState : IPlayerState
{
    public LandingState (PlayerController pc) : base("LandingState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    private bool busyLanding;
    public override void EnterState()
    {
        base.EnterState();
        _playerStateMachine.Animator.Play("Landing");
        busyLanding = true;
        _pc.StartCoroutine(Landing());
        
        _pc.canJump = true; _pc.canDoubleJump = true; _pc.jumpReleased = false;
        _pc.jumped = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!busyLanding) _playerStateMachine.ChangeState((_pc.IdleState));
       
    }
    public override void ExitState()
    {
        

    }
    
    public IEnumerator Landing()
    {
        yield return new WaitForSeconds(.15f);
        busyLanding = false;
    }
    
}