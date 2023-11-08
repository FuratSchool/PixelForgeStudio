using UnityEngine;

public class Falling : BaseState
{
    
    

    public Falling (MovementSM stateMachine) : base("Falling", stateMachine) {_sm = (MovementSM)this.stateMachine;}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered falling");
    }
    

    public override void UpdateLogic()
    {
        base.UpdateLogic();
       
        if (_sm.IsGrounded())
            stateMachine.ChangeState((_sm.idleState));
        if (_sm._canDash && _sm._dashPressed) 
            stateMachine.ChangeState(_sm.dashingState);
        if (_sm.SwingPressed && _sm._canSwing && _sm.InRange)
            stateMachine.ChangeState(_sm.swingingState);
        if (_sm.SpacePressed && _sm.canDoubleJump && _sm.jumpReleased)
            stateMachine.ChangeState(_sm.doubleJumpingState);
        
    }
    
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.rigidbody.transform.Translate(_sm.GetDirection(_sm.PlayerInput()).normalized * (_sm.speed * Time.deltaTime), 
            Space.World);
        
    }

}