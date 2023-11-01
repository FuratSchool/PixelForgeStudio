using UnityEngine;

public class WalkingState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerMovement = stateMachine.GetPlayerMovementController();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        _playerMovement.OnMove();

        if (!_playerController.IsPlayerMoving)
            stateMachine.ChangeState(new IdleState());

        if (_playerController.IsGrounded() && _playerController.canJump)
            if (Input.GetKey(KeyCode.Space))
                stateMachine.ChangeState(new JumpingState());

        if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
            stateMachine.ChangeState(new DashingState());

        if (Input.GetKeyDown(KeyCode.LeftShift)) _playerMovement.OnSprintStart();
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        // Cleanup or transition logic, if necessary
    }
}