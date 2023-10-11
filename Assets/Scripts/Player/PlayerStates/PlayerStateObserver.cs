using UnityEngine;

public class PlayerStateObserver : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    public void Subscribe(PlayerStateMachine stateMachine)
    {
        playerStateMachine = stateMachine;
        playerStateMachine.OnStateChanged += HandlePlayerStateChanged;
    }

    public void Unsubscribe(PlayerStateMachine stateMachine)
    {
        if (playerStateMachine == stateMachine)
        {
            playerStateMachine.OnStateChanged -= HandlePlayerStateChanged;
            playerStateMachine = null;
        }
    }

    private void HandlePlayerStateChanged(IPlayerState newState)
    {
       // Debug.Log($"Player's state changed to {newState.GetType().Name}");
    }
}