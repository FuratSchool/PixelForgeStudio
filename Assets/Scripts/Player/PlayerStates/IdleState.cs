using UnityEngine;

public class IdleState : IPlayerState
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
        if (_playerController.IsPlayerMoving)
            stateMachine.ChangeState(new WalkingState());

        if (Input.GetKeyDown(KeyCode.LeftShift)) _playerMovement.OnSprintStart();

        if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
            stateMachine.ChangeState(new DashingState());

        if (_playerController.IsGrounded() && _playerController.canJump)
            if (Input.GetKey(KeyCode.Space))
                stateMachine.ChangeState(new JumpingState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}