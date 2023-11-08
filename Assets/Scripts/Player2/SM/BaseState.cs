public class BaseState
{
    public string name;
    
    protected StateMachine stateMachine;
    protected MovementSM _sm;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
        _sm = (MovementSM)this.stateMachine;
    }

    public virtual void Enter() {}
    public virtual void UpdateLogic() {}

    public virtual void UpdatePhysics()
    {
        _sm.grounded = _sm.IsGrounded();
        if (_sm.grounded) _sm.canDoubleJump = true;
        if (_sm.grounded) _sm.jumpReleased = false;
    }
    public virtual void Exit() {}
    
    
    
    
    
}