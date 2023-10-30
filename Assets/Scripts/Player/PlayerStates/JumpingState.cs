using UnityEngine;

public class JumpingState : IPlayerState
{
    private readonly PlayerController _playerController;
    private JumpingController _jumpingController;

    public JumpingState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _jumpingController = new JumpingController(_playerController);
        _jumpingController.OnJump();
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        if (_playerController.PlayerInput().magnitude >= 0.1f && !_playerController.IsGrounded())
            playerStateMachine.GetPlayerMovementController().OnMove();

        if (_playerController.IsGrounded())
            playerStateMachine.ChangeState(new IdleState());

        if (Input.GetKeyDown(KeyCode.Q) && _playerController.CanDash)
            playerStateMachine.ChangeState(new DashingState());
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }
}