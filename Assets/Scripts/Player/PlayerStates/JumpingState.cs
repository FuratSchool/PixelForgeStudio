using System.Collections;
using UnityEngine;

public class JumpingState : IPlayerState
{
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        var playerController = stateMachine.GetPlayerController();
        this.stateMachine = stateMachine;
        var rb = stateMachine.GetPlayerController().GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * playerController.jumpForce, ForceMode.Impulse);
        stateMachine.StartCoroutine(JumpingCoroutine());
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        var playerController = playerStateMachine.GetPlayerController();
        if (playerController.IsGrounded()) stateMachine.ChangeState(new IdleState());
        /*if (!playerController.IsOnTerrain())
        {
            stateMachine.ChangeState(new DeathState());
        }*/
    }

    public void ExitState(PlayerStateMachine playerStateMachine)
    {
        stateMachine.StopAllCoroutines();
    }

    private IEnumerator JumpingCoroutine()
    {
        var playerController = stateMachine.GetPlayerController();
        var rb = playerController.rb;
        while (stateMachine.GetCurrentState() == this)
        {
            var moveDirection = (playerController.rb.transform.forward * playerController.GetVerticalInput() +
                                 rb.transform.right * playerController.GetHorizontalInput()).normalized;
            playerController.rb.velocity = new Vector3(moveDirection.x * playerController.moveSpeed, rb.velocity.y,
                moveDirection.z * playerController.moveSpeed);

            UpdateState(stateMachine);
            yield return null;
        }
    }
}