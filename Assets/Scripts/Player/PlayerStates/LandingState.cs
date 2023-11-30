using System.Collections;
using UnityEngine;

public class LandingState : IPlayerState
{
    public LandingState (PlayerController pc) : base("LandingState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    private bool busyLanding;
    public override void EnterState()
    {
        base.EnterState();
        //_playerStateMachine.Animator.Play("Landing");
        _playerStateMachine.Animator.SetInteger("State", 9);
        busyLanding = true;
        _pc.StartCoroutine(Landing());
        
        _pc.canJump = true; _pc.canDoubleJump = true; _pc.jumpReleased = false;
        _pc.jumped = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if ((Mathf.Abs(_pc.Movement.x) > Mathf.Epsilon) || (Mathf.Abs(_pc.Movement.y) > Mathf.Epsilon))
        {
            if (_pc._isRunning)
            {
                _playerStateMachine.ChangeState(_pc.SprintingState);
                return;
            }

            _playerStateMachine.ChangeState(_pc.WalkingState);
        }
        else if ((Mathf.Abs(_pc.Movement.x) < Mathf.Epsilon)&&(Mathf.Abs(_pc.Movement.y) < Mathf.Epsilon))
            _playerStateMachine.ChangeState(_pc.IdleState);
       
    }
    
    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);

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