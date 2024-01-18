using System;

using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{

    private PlayerState _currentState;
    private PlayerState _previousState;

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        _currentState = GetInitialState();
        if (_currentState != null)
            _currentState.EnterState();
    }
    
    public void Update()
    {
        if (_currentState != null) _currentState.UpdateState();
    }

    public void FixedUpdate()
    {
        if (_currentState != null) _currentState.FixedUpdateState();
    }

    public void LateUpdate()
    {
        if (_currentState != null) _currentState.LateUpdateState();
    }
    
    public void ChangeState(PlayerState newState)
    {
        Debug.Log($"Player's state changed to {newState.Name}");
        if (_currentState != null)
        {
            // Store the current state as the previous state
            _previousState = _currentState; 
            _currentState.ExitState();
        }

        _currentState = newState;
        _currentState.EnterState();
    }
    public PlayerState GetPreviousState()
    {
        return _previousState;
    }
    public PlayerState GetCurrentState()
    {
        return _currentState;
    }
    
    public Animator Animator { get; private set; }

    protected virtual PlayerState GetInitialState()
    {
        return null;
    }
}