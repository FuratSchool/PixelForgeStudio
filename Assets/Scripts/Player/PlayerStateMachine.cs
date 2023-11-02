using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private IPlayerState currentState;
    private IPlayerState previousState; // Add this variable to track the previous state

    public event Action<IPlayerState> OnStateChanged;

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            previousState = currentState; // Store the current state as the previous state
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
        OnStateChanged?.Invoke(currentState);
    }

    public void UpdateState()
    {
        if (currentState != null) currentState.UpdateState(this);
    }

    public IPlayerState GetPreviousState()
    {
        return previousState;
    }

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public PlayerController GetPlayerController()
    {
        return _playerController;
    }

    public void SetPlayerMovementController(PlayerMovementController playerMovement)
    {
        _playerMovement = playerMovement;
    }

    public PlayerMovementController GetPlayerMovementController()
    {
        return _playerMovement;
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }
}