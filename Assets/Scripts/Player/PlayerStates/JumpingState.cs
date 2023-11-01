public class JumpingState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        if (_playerController.PlayerInput().magnitude >= 0.1f && !_playerController.IsGrounded())
            playerStateMachine.GetPlayerMovementController().OnMove();

        // if (_playerController.IsGrounded())
        //     playerStateMachine.ChangeState(new IdleState());
        //
        // if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
        //     playerStateMachine.ChangeState(new DashingState());
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }
}