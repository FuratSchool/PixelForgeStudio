using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private PlayerStateMachine _stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _playerController = stateMachine.GetPlayerController();
        _playerMovement = stateMachine.GetPlayerMovementController();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if ((_playerController != null && Mathf.Abs(_playerController.HorizontalInput) > 0.1f) ||
            Mathf.Abs(_playerController.VerticalInput) > 0.1f)
            _stateMachine.ChangeState(new WalkingState());

        if (Input.GetKeyDown(KeyCode.LeftShift)) _playerMovement.OnSprintStart();

        if (_playerController.IsGrounded() && _playerController.CanJump)
            if (Input.GetKey(KeyCode.Space))
                stateMachine.ChangeState(new JumpingState(_playerController));
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}