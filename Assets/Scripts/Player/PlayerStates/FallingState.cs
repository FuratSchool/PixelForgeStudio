using Unity.VisualScripting;
using UnityEngine;
    public class FallingState : PlayerState
    {
        public FallingState(PlayerController pc) : base("FallingState", pc)
        {
            PC = (PlayerController)this.PlayerStateMachine;
        }
        public override void EnterState()
        {
            PlayerStateMachine.Animator.SetInteger("State", 6);
            if (!PC.jumped) PC.jumpReleased = true;
            PC.EnableGrimParticles(false);
        }
        
        public override void UpdateState()
        {
            base.UpdateState();

            if (PC.IsGrounded())
            {
                PC.MoveSpeed = PC.WalkSpeed;
                PlayerStateMachine.ChangeState((PC.LandingState));
            }
            else if (PC._canDash && PC.dashPressed) 
                PlayerStateMachine.ChangeState(PC.DashingState);
            else if (PC.SwingPressed && PC._canSwing && PC.InRange)
                PlayerStateMachine.ChangeState(PC.SwingingState);
            else if (PC.SpacePressed && PC.canDoubleJump && PC.jumpReleased)
                PlayerStateMachine.ChangeState(PC.DoubleJumpState);
            
            if (CheckSwing())
            {
                EnableSwingText(PC.GetUIController(), PC.GetPlayerInput());
                if (PC.SwingPressed)
                {
                    PlayerStateMachine.ChangeState(PC.SwingingState);
                    return;
                }
            }
            else
            {
                DisableSwingText(PC.GetUIController());
            }
            
        }
        
        public override void ExitState()
        {
            PC.EnableGrimParticles(true);
        }
        public override void LateUpdateState()
        {
            base.LateUpdateState();

            _pc.GetRigidbody().AddForce(Physics.gravity*_pc.gravityMultiplier);
            if(_pc.EnableMovement){
                _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * ((_pc.MoveSpeed * _pc.SpeedBoostMultiplier) * Time.deltaTime), 
                    Space.World);
            }
// Changed Today (16-1-2024)
//            PC.GetRigidbody().AddForce(Physics.gravity*PC.gravityMultiplier);
//            PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
//                Space.World);

        }
    }