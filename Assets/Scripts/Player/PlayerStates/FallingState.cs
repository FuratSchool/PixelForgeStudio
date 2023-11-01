using UnityEngine;

namespace Player.PlayerStates
{
    public class FallingState : IPlayerState
    {
        private PlayerController _playerController;

        public void EnterState(PlayerStateMachine stateMachine)
        {
            _playerController = stateMachine.GetPlayerController();
        }

        public void UpdateState(PlayerStateMachine stateMachine)
        {
            if (_playerController.PlayerInput().magnitude >= 0.1f && !_playerController.IsGrounded())
                stateMachine.GetPlayerMovementController().OnMove();
            if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
                stateMachine.ChangeState(new DashingState());
            if (_playerController.IsGrounded()) stateMachine.ChangeState(new IdleState());
        }

        public void ExitState(PlayerStateMachine stateMachine)
        {
        }
    }
}