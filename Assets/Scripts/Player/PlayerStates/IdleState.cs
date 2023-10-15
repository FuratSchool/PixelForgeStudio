using System.Collections;
using UnityEngine;

// Idle State Logic
public class IdleState : IPlayerState
{
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        stateMachine.StartCoroutine(IdleCoroutine());
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        var playerController = this.stateMachine.GetPlayerController();

        if (Mathf.Abs(playerController.GetHorizontalInput()) > 0.1f ||
            Mathf.Abs(playerController.GetVerticalInput()) > 0.1f) stateMachine.ChangeState(new WalkingState());

        /*if (!playerController.IsOnTerrain())
        {
            stateMachine.ChangeState(new DeathState());
        }*/
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.StopAllCoroutines();
    }

    private IEnumerator IdleCoroutine()
    {
        while (stateMachine.GetCurrentState() == this)
        {
            UpdateState(stateMachine);
            yield return null;
        }
    }
}