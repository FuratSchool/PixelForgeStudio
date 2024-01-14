using UnityEngine.InputSystem;

public class PlayerState
{
    public string Name;
    protected PlayerStateMachine PlayerStateMachine;
    protected PlayerController PC;
    
    public PlayerState(string name,PlayerStateMachine playerStateMachine)
    {
        this.Name = name;
        this.PlayerStateMachine = playerStateMachine;
        PC = (PlayerController)this.PlayerStateMachine;
    }
    public virtual void EnterState() { }

    public virtual void UpdateState()
    {
        IsTransitioning();
        if (!PC.SpacePressed) PC.CanJumpAgain = true;
        if (PC.GetRigidbody().transform.position.y < -40 || PC.touchedWater)
        {
            PlayerStateMachine.ChangeState(PC.DeathState);
        }

        PC.MoveSpeed = PC.isRunning ? PC.SprintSpeed : PC.WalkSpeed;
    }
    public virtual void FixedUpdateState() { }

    public virtual void LateUpdateState()
    {
        PC.grounded = PC.IsGrounded();
        if (PC.IsGrounded())
        {
            PC.canJump = true;
            PC.canDoubleJump = true;
        }
    }
    public virtual void ExitState() { }
    
    public void EnableInteractDialogueActive(UIController uiController, PlayerInput playerInput, string text)
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
        uiController.SetInteractText("Press "+ keybind +text);
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
        if(!PC.IsSwinging && PC.canSwing && PC.inSwingingRange)
            return true;
        return false;
    }
    
    public bool IsTransitioning()
    {
        if (PC.isTransitioning && PlayerStateMachine.GetCurrentState() != PC.TransitionState)
            PC.ChangeState(PC.TransitionState);
        return false;
    }
    
}