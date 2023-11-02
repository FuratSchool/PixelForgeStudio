using UnityEngine;

public class DashingState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private Rigidbody _rigidbody;
    private float dashStartTime;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerMovement = stateMachine.GetPlayerMovementController();
        _rigidbody = _playerController.GetRigidbody();
        dashStartTime = Time.time;
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        if (Time.time - dashStartTime >= _playerController.DashingTime) playerStateMachine.ChangeState(new IdleState());
        if (Time.time - dashStartTime >= _playerController.DashingTime && _playerController.IsPlayerMoving)
            playerStateMachine.ChangeState(new WalkingState());
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }
}