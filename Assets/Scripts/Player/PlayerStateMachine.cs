using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public event Action<IPlayerState> OnStateChanged;
    private IPlayerState currentState;
    private PlayerController _playerController;
    
    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
        OnStateChanged?.Invoke(currentState);
    }

    public void UpdateState()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public PlayerController GetPlayerController()
    {
        return _playerController;
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }
    
}