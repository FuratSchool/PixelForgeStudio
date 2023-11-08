public class IPlayerState
{
    public string name;
    protected PlayerStateMachine _playerStateMachine;
    protected PlayerController _pc;

    public IPlayerState(string name,PlayerStateMachine playerStateMachine)
    {
        this.name = name;
        this._playerStateMachine = playerStateMachine;
        _pc = (PlayerController)this._playerStateMachine;
    }
    public virtual void EnterState() { }

    public virtual void UpdateState()
    {
        if (_pc.GetRigidbody().transform.position.y < -40)
        {
            _playerStateMachine.ChangeState(_pc.DeathState);
        }
    }
    public virtual void FixedUpdateState() { }

    public virtual void LateUpdateState()
    {
        if (_pc.IsGrounded()){ _pc.canDoubleJump = true; _pc.jumpReleased = false;}
    }
    public virtual void ExitState() { }
}