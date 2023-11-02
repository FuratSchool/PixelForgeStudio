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
            if (_playerController.IsPlayerMoving && !_playerController.IsGrounded())
                stateMachine.GetPlayerMovementController().OnMove();

            if (_playerController.IsGrounded() && !_playerController.IsPlayerMoving)
                stateMachine.ChangeState(new IdleState());

            if (_playerController.IsGrounded() && _playerController.IsPlayerMoving)
                stateMachine.ChangeState(new WalkingState());


            if (Input.GetKeyDown(KeyCode.Q) && _playerController.canDash)
                stateMachine.ChangeState(new DashingState());
        }

        public void ExitState(PlayerStateMachine stateMachine)
        {
        }
    }
}