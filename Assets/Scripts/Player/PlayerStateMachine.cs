using System;
using Player.PlayerStates;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerController _playerController;
    private IPlayerState currentState;
    private Animator _animator;

    public WalkingState WalkingState = new WalkingState();
    public IdleState IdleState = new IdleState();
    public DashingState DashingState = new DashingState();
    public JumpingState JumpingState = new JumpingState();
    public FallingState FallingState = new FallingState();
    public SwingingState SwingingState = new SwingingState();
    public SprintingState SprintingState = new SprintingState();
    public TalkingState TalkingState = new TalkingState();
    public TransitionState TransitionState = new TransitionState();
    public DeathState DeathState = new DeathState();
    
    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    
    public IPlayerState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    private IPlayerState previousState; // Add this variable to track the previous state

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            previousState = currentState; // Store the current state as the previous state
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void Update()
    {
        _playerController.IsOnTerrain();
        _playerController.IsTransitioning();
        if (currentState != null) currentState.UpdateState(this);
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.FixedUpdateState(this);
    }

    public void LateUpdate()
    {
        if (currentState != null) currentState.LateUpdateState(this);
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

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }
    
    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }
}