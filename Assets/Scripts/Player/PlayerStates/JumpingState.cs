using Player.PlayerStates;
using UnityEngine;

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
        {
            if (_playerController._swingingComponent.InRange) // Check if the player is in range of the swinging object
            {
                Debug.Log("ts");

                playerStateMachine.ChangeState(new SwingingState());
            }
        }
        else
        {
            playerStateMachine.ChangeState(new FallingState());
        }
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
    }
}