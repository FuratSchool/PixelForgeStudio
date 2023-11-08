using UnityEngine;

public class Idle : BaseState
{
    
    

    public Idle (MovementSM stateMachine) : base("Idle", stateMachine) {_sm = (MovementSM)this.stateMachine;}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Idle");
    }
    

    public override void UpdateLogic()
    {
        base.UpdateLogic();
       
        if(!_sm.canMove)
            stateMachine.ChangeState(_sm.dialogueState);
        if ((Mathf.Abs(_sm._movement.x) > Mathf.Epsilon)||(Mathf.Abs(_sm._movement.y) > Mathf.Epsilon))
            stateMachine.ChangeState(_sm.walkingState);
        if (_sm.SpacePressed && _sm.canJump)
            stateMachine.ChangeState((_sm.jumpingState));
        if (_sm._canDash && _sm._dashPressed) 
            stateMachine.ChangeState(_sm.dashingState);
        if (_sm.SwingPressed && _sm._canSwing && _sm.InRange)
            stateMachine.ChangeState(_sm.swingingState);
    }

}