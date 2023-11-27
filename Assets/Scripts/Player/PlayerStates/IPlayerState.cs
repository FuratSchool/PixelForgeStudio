using UnityEngine.InputSystem;

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
        IsTransitioning();
        
        if (_pc.GetRigidbody().transform.position.y < -40)
        {
            _playerStateMachine.ChangeState(_pc.DeathState);
        }
    }
    public virtual void FixedUpdateState() { }

    public virtual void LateUpdateState()
    {
        _pc.grounded = _pc.IsGrounded();
        if (_pc.IsGrounded())
        {
            

        }
    }
    public virtual void ExitState() { }
    
    public void EnableInteractDialogueActive(UIController uiController, PlayerInput playerInput)
    {
        string keybind;
        if (playerInput.currentControlScheme.Equals("Controller"))
        {
            int index = playerInput.actions["Interact"].bindings.IndexOf(x => x.groups.Contains("Controller"));
            keybind = playerInput.actions["Interact"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        else
        {
            int index = playerInput.actions["Interact"].bindings.IndexOf(x => x.groups.Contains("KeyboardMouse"));
            keybind = playerInput.actions["Interact"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        uiController.SetInteractText("Press "+ keybind +" to talk");
        uiController.SetInteractableTextActive(true);
    }
    public void DisableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
    public void EnableSwingText(UIController uiController, PlayerInput playerInput)
    {
        string keybind;
        if (playerInput.currentControlScheme.Equals("Controller"))
        {
            int index = playerInput.actions["Swing"].bindings.IndexOf(x => x.groups.Contains("Controller"));
            keybind = playerInput.actions["Swing"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        else
        {
            int index = playerInput.actions["Swing"].bindings.IndexOf(x => x.groups.Contains("KeyboardMouse"));
            keybind = playerInput.actions["Swing"].GetBindingDisplayString(index, out var deviceLayoutName, out var controlPath);
        }
        
        uiController.SetInteractText("Hold " + keybind +" to swing");
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
    
    public bool IsTransitioning()
    {
        if (_pc.isTransitioning && _playerStateMachine.GetCurrentState() != _pc.TransitionState)
            _pc.ChangeState(_pc.TransitionState);
        return false;
    }
    
}