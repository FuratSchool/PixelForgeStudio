using UnityEngine;

public class DoubleJumping : BaseState
{
    

    private bool _grounded;
    private float distToGround;
    

    public DoubleJumping(MovementSM stateMachine) : base("DoubleJumping", stateMachine)
    {
        _sm = (MovementSM)this.stateMachine;
    }
    
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("DoubleJumping");
        _sm.rigidbody.AddForce(Vector3.up * (_sm.force * 1.5f), ForceMode.Impulse);
        _sm.canDoubleJump = false;
        _sm.jumpReleased = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        stateMachine.ChangeState(_sm.fallingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
        
    }

}