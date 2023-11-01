using UnityEngine;

public class SprintingState : IPlayerState
{
    private readonly float runSpeed = 8.0f;
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;

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
            stateMachine.ChangeState(new IdleState());

        if (_playerController.IsPlayerMoving && !Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(new WalkingState());

        if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
            stateMachine.ChangeState(new DashingState());

        if (_playerController.IsGrounded() && _playerController.canJump)
            if (Input.GetKey(KeyCode.Space))
                stateMachine.ChangeState(new JumpingState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        _playerMovement.OnSprintFinish();
    }
}