using Unity.VisualScripting;
using UnityEngine;
    public class FallingState : IPlayerState
    {
        public FallingState(PlayerController pc) : base("FallingState", pc)
        {
            _pc = (PlayerController)this._playerStateMachine;
        }
        public override void EnterState()
        {
            _playerStateMachine.Animator.Play("Falling");
            

        }
        public override void UpdateState()
        {
            base.UpdateState();
       
            if (_pc.IsGrounded())
                _playerStateMachine.ChangeState((_pc.IdleState));
            if (_pc._canDash && _pc.dashPressed) 
                _playerStateMachine.ChangeState(_pc.DashingState);
            if (_pc.SwingPressed && _pc._canSwing && _pc.InRange)
                _playerStateMachine.ChangeState(_pc.SwingingState);
            if (_pc.SpacePressed && _pc.canDoubleJump && _pc.jumpReleased)
                _playerStateMachine.ChangeState(_pc.DoubleJumpState);
            
            if (CheckSwing())
            {
                EnableSwingText(_pc.GetUIController());
                if (_pc.SwingPressed)
                {
                    _playerStateMachine.ChangeState(_pc.SwingingState);
                }
            }
            else
            {
                DisableSwingText(_pc.GetUIController());
            }
            
        }
        
        public override void ExitState()
        {
            //_playerStateMachine.Animator.SetBool("IsFalling", false);        

            //stateMachine.SwingingState.DisableSwingText(_playerController.GetUIController());

        }
        public override void LateUpdateState()
        {
            base.LateUpdateState();
            _pc.GetRigidbody().AddForce(Physics.gravity*1.5f);
            _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
                Space.World);
            
        }
    }