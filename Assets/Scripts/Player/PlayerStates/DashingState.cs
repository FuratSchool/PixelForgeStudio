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

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (Time.time - dashStartTime >= 3f) stateMachine.ChangeState(new IdleState());
        if (_playerMovement.IsDashing == false && _playerController.IsGrounded() && _playerController.IsPlayerMoving)
        {
            stateMachine.ChangeState(new WalkingState());
        }
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}