using Player.PlayerStates;
using UnityEngine;

public class JumpingState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (!_playerController.IsGrounded() && stateMachine.GetCurrentState() is JumpingState)
        {

            if (_playerController._swingingComponent.InRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("In range of the swinging object");
                stateMachine.ChangeState(new SwingingState());
            }
        }

        if (!_playerController.IsGrounded())
        {
            stateMachine.ChangeState(new FallingState());
        }
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}