using Player.PlayerStates;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : IPlayerState
{
    private PlayerController _playerController;
    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerController.IsJumping = true;
        StartJump();
        if (_playerController.ShiftPressed)
        {
            _playerController.MoveSpeed = _playerController.SprintSpeed;
        }
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        stateMachine.WalkingState.PlayerMove(stateMachine);
        if (!IsGrounded(stateMachine) && _playerController.GetRigidbody().velocity.y < -0.1f && !_playerController.IsJumping)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
        else if (_playerController.canDash && _playerController.DashPressed)
            stateMachine.ChangeState(stateMachine.DashingState);
        else if (stateMachine.SwingingState.CheckSwing(stateMachine) && !IsGrounded(stateMachine))
        {
            stateMachine.SwingingState.EnableSwingText(_playerController.GetUIController());
            if (_playerController.SwingPressed)
            {
                stateMachine.ChangeState(stateMachine.SwingingState);
            }
        }
        else
        {
            stateMachine.SwingingState.DisableSwingText(_playerController.GetUIController());
        }
    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
        ContinueJump();
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        
        _playerController.IsJumping = false;
        _playerController.jumpTimeCounter = 0;
        stateMachine.SwingingState.DisableSwingText(_playerController.GetUIController());
    }
    
    private void StartJump()
    {
        _playerController.jumpTimeCounter = _playerController.jumpTime;
        _playerController.GetRigidbody().AddForce(Vector3.up * _playerController.force, ForceMode.Impulse);
    }
    
    private void ContinueJump()
    {
        if (_playerController.jumpTimeCounter > 0 && _playerController.SpacePressed)
        {
            _playerController.GetRigidbody().AddForce(Vector3.up * _playerController.forceHoldJump, ForceMode.Impulse);
            _playerController.jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            _playerController.IsJumping = false;
        }
    }
    public bool IsGrounded(PlayerStateMachine stateMachine)
    {
        int layermask = 1 << 6;
        var controller = stateMachine.GetPlayerController();
        return Physics.Raycast(controller.transform.position, Vector3.down, controller.raycastDistance, ~layermask); 
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}