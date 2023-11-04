using UnityEngine;

public class SprintingState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerController.MoveSpeed = _playerController.SprintSpeed;
    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        stateMachine.WalkingState.PlayerMove(stateMachine);
        if (stateMachine.JumpingState.IsGrounded(stateMachine) && _playerController.SpacePressed)
            stateMachine.ChangeState(stateMachine.JumpingState);
        
        else if (_playerController.IsPlayerMoving && !_playerController.ShiftPressed)
            stateMachine.ChangeState(stateMachine.WalkingState);

        else if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
            stateMachine.ChangeState(stateMachine.DashingState);
        else if (_playerController.canDash && _playerController.DashPressed)
            stateMachine.ChangeState(stateMachine.DashingState);
        else if (!_playerController.IsPlayerMoving)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        _playerController.MoveSpeed = _playerController.WalkSpeed;
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}