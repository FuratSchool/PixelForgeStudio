using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (stateMachine.JumpingState.IsGrounded(stateMachine) && _playerController.SpacePressed)
            stateMachine.ChangeState(stateMachine.JumpingState);
        else if (!stateMachine.JumpingState.IsGrounded(stateMachine))
            stateMachine.ChangeState(stateMachine.FallingState);
        else if (_playerController.IsPlayerMoving)
            stateMachine.ChangeState(stateMachine.WalkingState);
        else if (_playerController.ShiftPressed)
            stateMachine.ChangeState(stateMachine.SprintingState);
    }
    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}