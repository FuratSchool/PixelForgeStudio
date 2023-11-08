using System;

using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{

    IPlayerState currentState;
    IPlayerState previousState; // Add this variable to track the previous state
    private Animator _animator;

    
    
    
   

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        currentState = GetInitialState();
        if (currentState != null)
            currentState.EnterState();
    }
    
    public void Update()
    {
        //_playerController.IsOnTerrain();
        //_playerController.IsTransitioning();
        if (currentState != null) currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.FixedUpdateState();
    }

    public void LateUpdate()
    {
        if (currentState != null) currentState.LateUpdateState();
    }
    
    public void ChangeState(IPlayerState newState)
    {
        Debug.Log($"Player's state changed to {newState.name}");
        if (currentState != null)
        {
            previousState = currentState; // Store the current state as the previous state
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
    }

    

    

    public IPlayerState GetPreviousState()
    {
        return previousState;
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }
    
    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }
    protected virtual IPlayerState GetInitialState()
    {
        return null;
    }
}