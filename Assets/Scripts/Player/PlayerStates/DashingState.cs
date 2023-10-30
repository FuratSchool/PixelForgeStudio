using UnityEngine;

public class DashingState : IPlayerState
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

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        Dash(_playerMovement.GetDirection(_playerController
            .PlayerInput()));

        if (_playerController.IsGrounded()) playerStateMachine.ChangeState(new IdleState());
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }

    public void Dash(Vector3 dashDirection)
    {
        var dashForce = dashDirection.normalized * _playerController.DashingPower;
        _rigidbody.AddForce(dashForce, ForceMode.VelocityChange);
    }
}