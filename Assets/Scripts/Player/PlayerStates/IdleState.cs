using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
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
        else if (_playerController.ShiftPressed)
            stateMachine.ChangeState(stateMachine.SprintingState);
        if (_playerController.InDialogeTriggerZone && _playerController.NPC.hasBeenTalkedTo == false)
        {
            stateMachine.TalkingState.EnableInteractDialogueActive(_playerController.GetUIController());
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
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}