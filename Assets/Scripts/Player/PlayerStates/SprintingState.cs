using UnityEngine;

public class SprintingState : IPlayerState
{
    private readonly float runSpeed = 8.0f;
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private PlayerStateMachine _stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerMovement = stateMachine.GetPlayerMovementController();
        _playerController.MoveSpeed = 12f;
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        _playerMovement.OnMove();
        if (!_playerController.IsPlayerMoving)
            _stateMachine.ChangeState(new IdleState());
        if (Input.GetKeyDown(KeyCode.Q) && _playerController.CanDash)
            stateMachine.ChangeState(new DashingState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        _playerMovement.OnSprintFinish();
    }
}