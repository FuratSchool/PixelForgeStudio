using UnityEngine;

public class Dashing : BaseState
{


    public Dashing (MovementSM stateMachine) : base("Dashing", stateMachine) {_sm = (MovementSM)this.stateMachine;}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Dash");
        _sm.StartCoroutine(_sm.Dash());
    }
    
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.IsGrounded() && _sm._isDashing)
        {
            _sm._dashPressed = false;
            stateMachine.ChangeState((_sm.fallingState));
        }

        if (_sm.IsGrounded() && _sm._isDashing)
        {
            _sm._dashPressed = false;
            stateMachine.ChangeState((_sm.idleState));
        }

    }
    
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        

    }
    
    
}
