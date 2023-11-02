using UnityEngine;

namespace Player.PlayerStates
{
    public class FallingState : IPlayerState
    {
        private PlayerController _playerController;
        private PlayerMovementController _playerMovement;

        public void EnterState(PlayerStateMachine stateMachine)
        {
            _playerController = stateMachine.GetPlayerController();
            _playerMovement = stateMachine.GetPlayerMovementController();
        }

        public void UpdateState(PlayerStateMachine stateMachine)
        {
            if (_playerController.IsPlayerMoving && !_playerController.IsGrounded())
                stateMachine.GetPlayerMovementController().OnMove();

            if (_playerController._swingingComponent.InRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("In range of the swinging object");
                stateMachine.ChangeState(new SwingingState());
            }

            if (_playerController.IsGrounded() && !_playerMovement.SpacePressed)
                stateMachine.ChangeState(new IdleState());

            if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash) stateMachine.ChangeState(new DashingState());
        }


        public void ExitState(PlayerStateMachine stateMachine)
        {
        }
    }
}