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
    
    public void EnableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractText("Press [F][X] to talk");
        uiController.SetInteractableTextActive(true);
    }
    
    public void DisableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
    
    public void EnableSwingText(UIController uiController)
    {
        uiController.SetInteractText("Hold [E][Y] to swing");
        uiController.SetInteractableTextActive(true);
    }
    
    public void DisableSwingText(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
    
    public bool CheckSwing()
    {
        if(!_pc.IsSwinging && _pc.canSwing && _pc.inSwingingRange)
            return true;
        return false;
    }
    
    
}