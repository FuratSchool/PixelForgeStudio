using System.Collections;
using UnityEngine;

public class JumpingState : IPlayerState
{
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        PlayerController playerController = stateMachine.GetPlayerController();
        this.stateMachine = stateMachine;
        Rigidbody rb = stateMachine.GetPlayerController().GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * playerController.jumpForce, ForceMode.Impulse);
        stateMachine.StartCoroutine(JumpingCoroutine());
    }

    public void UpdateState(PlayerStateMachine playerStateMachine)
    {
        PlayerController playerController = playerStateMachine.GetPlayerController();
        if (playerController.IsGrounded())
        {
            stateMachine.ChangeState(new IdleState());
        }
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
        PlayerController playerController = this.stateMachine.GetPlayerController();
        var rb = playerController.rb;
        while (stateMachine.GetCurrentState() == this)
        {
            Vector3 moveDirection = (playerController.rb.transform.forward * playerController.VerticalInput + rb.transform.right * playerController.HorizontalInput).normalized;
            playerController.rb.velocity = new Vector3(moveDirection.x * playerController.moveSpeed, rb.velocity.y, moveDirection.z * playerController.moveSpeed);

            UpdateState(stateMachine);
            yield return null;
        }
    }
}