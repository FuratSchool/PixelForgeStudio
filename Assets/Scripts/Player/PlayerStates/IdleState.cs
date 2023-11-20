using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        stateMachine.Animator.SetBool("IsIdle", true);        

    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (stateMachine.JumpingState.IsGrounded(stateMachine) && _playerController.SpacePressed)
            stateMachine.ChangeState(stateMachine.JumpingState);
        else if (!stateMachine.JumpingState.IsGrounded(stateMachine))
            stateMachine.ChangeState(stateMachine.FallingState);
        else if (_playerController.IsPlayerMoving)
            stateMachine.ChangeState(stateMachine.WalkingState);
        if (_playerController.InDialogeTriggerZone && _playerController.NPC.hasBeenTalkedTo == false)
        {
            stateMachine.TalkingState.EnableInteractDialogueActive(stateMachine);
            if (_playerController.InteractPressed)
            {
                stateMachine.ChangeState(stateMachine.TalkingState);
            }
        }
        else
        {
            stateMachine.TalkingState.DisableInteractDialogueActive(_playerController.GetUIController());
        }
    }
    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.TalkingState.DisableInteractDialogueActive(_playerController.GetUIController());
        stateMachine.Animator.SetBool("IsIdle", false);        

    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}