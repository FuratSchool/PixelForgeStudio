using System.Collections;
using UnityEngine;

public class DeathState : IPlayerState
{
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        stateMachine.StartCoroutine(DeathCoroutine());
        Debug.Log("entered death state");
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        Vector3 spawnPoint = PlayerStatus.playerStatus.GetSpawnPoint();
        stateMachine.transform.position = spawnPoint;

        stateMachine.ChangeState(new IdleState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.StopAllCoroutines();
    }

    private IEnumerator DeathCoroutine()
    {
        while (stateMachine.GetCurrentState() == this)
        {
            UpdateState(stateMachine);
            yield return null;
        }
    }
}