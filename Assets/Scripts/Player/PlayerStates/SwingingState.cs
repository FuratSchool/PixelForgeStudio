using UnityEngine;

public class SwingingState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private Rigidbody _rigidbody;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerMovement = stateMachine.GetPlayerMovementController();
        _rigidbody = _playerController.GetRigidbody();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (_playerController.IsGrounded() && !_playerController.IsPlayerMoving)
            stateMachine.ChangeState(new IdleState());

        if (_playerController.IsGrounded() && _playerController.IsPlayerMoving)
            stateMachine.ChangeState(new WalkingState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}