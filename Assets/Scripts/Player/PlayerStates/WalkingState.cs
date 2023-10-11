using UnityEngine;
using System.Collections;

public class WalkingState : IPlayerState
{
    private float transitionDelay = 0.2f;
    private bool canExit = true;
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        stateMachine.StartCoroutine(WalkingCoroutine());
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        
        PlayerController playerController = stateMachine.GetPlayerController();
        Rigidbody rb = playerController.GetRigidbody();
        
        // Handle movement logic for walking state
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0f; // Ensure no vertical movement while walking

        // Apply movement
        Rigidbody playerRigidbody = playerController.GetRigidbody();
        playerRigidbody.velocity = moveDirection.normalized * playerController.GetMoveSpeed();

        if (Input.GetButton("Jump"))
        {
            HandleJump(stateMachine);
        }
        /*if (!playerController.IsOnTerrain())
        {
            stateMachine.ChangeState(new DeathState());
        }*/
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.StopAllCoroutines();
    }

    private void HandleJump(PlayerStateMachine stateMachine)
    {
        stateMachine.ChangeState(new JumpingState());
    }

    private IEnumerator WalkingCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(transitionDelay);

            if (canExit)
            {
                stateMachine.ChangeState(new IdleState());
                yield break;
            }
        }
    }
}