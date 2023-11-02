using Player.PlayerStates;

public class JumpingState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        if (!_playerController.IsGrounded() && playerStateMachine.GetCurrentState() is JumpingState)
            playerStateMachine.ChangeState(new FallingState());

        if (_playerController.IsPlayerMoving && _playerController.IsGrounded())
            playerStateMachine.GetPlayerMovementController().OnMove();
        //
        // if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
        //     playerStateMachine.ChangeState(new DashingState());
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }
}