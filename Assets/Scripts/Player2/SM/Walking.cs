using UnityEngine;

public class Walking : BaseState
{
    

    public Walking(MovementSM stateMachine) : base("Walking", stateMachine)
    {
        _sm = (MovementSM) stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered moving");
        _sm.speed = _sm.normalSpeed;
    }

    
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        
        if ((Mathf.Abs(_sm._movement.x) < Mathf.Epsilon)&&(Mathf.Abs(_sm._movement.y) < Mathf.Epsilon))
            stateMachine.ChangeState(_sm.idleState);
        if (_sm.SpacePressed && _sm.canJump)
            stateMachine.ChangeState((_sm.jumpingState));
        if (_sm._isRunning)
            stateMachine.ChangeState(_sm.runningState);
        if (_sm._canDash && _sm._dashPressed) 
            stateMachine.ChangeState(_sm.dashingState);
        if (_sm.SwingPressed && _sm._canSwing && _sm.InRange)
            stateMachine.ChangeState(_sm.swingingState);
        if(!_sm.grounded)
            stateMachine.ChangeState(_sm.fallingState);
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.rigidbody.transform.Translate(_sm.GetDirection(_sm.PlayerInput()).normalized * (_sm.speed * Time.deltaTime), 
            Space.World);
        _sm.FootPrint(.3f);
        
    }

}