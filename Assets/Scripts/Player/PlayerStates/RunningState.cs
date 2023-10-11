using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : IPlayerState
{
    private float runSpeed = 8.0f;
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        stateMachine.StartCoroutine(RunningCoroutine());
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        Vector3 moveDirection = Vector3.forward;
        stateMachine.transform.Translate(moveDirection * (runSpeed * Time.deltaTime));

        if (Input.GetButtonDown("Jump"))
        {
            stateMachine.ChangeState(new JumpingState());
        }
        else if (Input.GetButtonDown("Walk"))
        {
            stateMachine.ChangeState(new WalkingState());
        }
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.StopAllCoroutines();
    }

    private IEnumerator RunningCoroutine()
    {
        while (stateMachine.GetCurrentState() == this)
        {
            UpdateState(stateMachine);
            yield return null;
        }
    }
}