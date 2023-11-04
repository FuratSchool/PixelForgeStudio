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
        public void FixedUpdateState(PlayerStateMachine stateMachine)
        {
        }
        public void UpdateState(PlayerStateMachine stateMachine)
        {
            stateMachine.WalkingState.PlayerMove(stateMachine);
            if (stateMachine.JumpingState.IsGrounded(stateMachine))
            {
                if (_playerController.ShiftPressed && _playerController.IsPlayerMoving)
                    stateMachine.ChangeState(stateMachine.SprintingState);
                else if (_playerController.IsPlayerMoving)
                    stateMachine.ChangeState(stateMachine.WalkingState);
                else
                    stateMachine.ChangeState(stateMachine.IdleState);
            }
            else if (stateMachine.SwingingState.CheckSwing(stateMachine) && !stateMachine.JumpingState.IsGrounded(stateMachine))
            {
                stateMachine.SwingingState.EnableSwingText(_playerController.GetUIController());
                if (_playerController.SwingPressed)
                {
                    stateMachine.ChangeState(stateMachine.SwingingState);
                }
            }
            else if (_playerController.canDash && _playerController.DashPressed)
                stateMachine.ChangeState(stateMachine.DashingState);
            else
            {
                stateMachine.SwingingState.DisableSwingText(_playerController.GetUIController());
            }
            
        }
        
        public void ExitState(PlayerStateMachine stateMachine)
        {
            stateMachine.SwingingState.DisableSwingText(_playerController.GetUIController());
        }
        public void LateUpdateState(PlayerStateMachine stateMachine)
        {}
    }
}