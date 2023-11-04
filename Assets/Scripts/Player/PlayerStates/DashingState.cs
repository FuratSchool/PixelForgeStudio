using System.Collections;
using UnityEngine;

public class DashingState : IPlayerState
{
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private bool _isDashing;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerController.DashPressed = false;
        _rigidbody = _playerController.GetRigidbody();
        _rigidbody.useGravity = false;
        _isDashing = true;
        _playerController.canDash = false;
        _rigidbody.velocity = stateMachine.WalkingState.GetDirection(stateMachine.WalkingState._lastDirection, stateMachine).normalized * _playerController.dashSpeed;
        _playerController.StartCoroutine(Dash());
    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (_isDashing == false)
        {
            if(!stateMachine.JumpingState.IsGrounded(stateMachine))
                stateMachine.ChangeState(stateMachine.FallingState);
            else if (_playerController.ShiftPressed && _playerController.IsPlayerMoving)
                stateMachine.ChangeState(stateMachine.SprintingState);
            else if (_playerController.IsPlayerMoving)
                stateMachine.ChangeState(stateMachine.WalkingState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
            
        }
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
    
    private IEnumerator Dash()
    {
        _playerController.TR.emitting = true;
        yield return new WaitForSeconds(_playerController.dashingTime);
        _playerController.TR.emitting = false;
        _rigidbody.useGravity = true;
        _isDashing = false;
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        yield return new WaitForSeconds(_playerController.dashingCooldown);
        _playerController.canDash = true;
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}