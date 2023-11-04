using UnityEngine;

public class TalkingState : IPlayerState
{
    private PlayerController _playerController;
    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _playerController.NPC.hasBeenTalkedTo = true;
        _playerController.GetDialogueManager().StartDialogue(_playerController.NPC.dialogue);
        _playerController.DialogueActive = true;
        
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        if (!_playerController.DialogueActive)
            stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }

    public void LateUpdateState(PlayerStateMachine stateMachine)
    {
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        DisableInteractDialogueActive(_playerController.GetUIController());
        _playerController.NPC.hasBeenTalkedTo = true;
        _playerController.SpacePressed = false;
        if(_playerController.NPC.canTalkAgain)
            _playerController.NPC.hasBeenTalkedTo = false;
    }
    
    public void EnableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractText("Press F to talk");
        uiController.SetInteractableTextActive(true);
    }
    public void DisableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
}