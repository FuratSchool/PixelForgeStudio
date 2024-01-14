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
            
            else if (CheckSwing())
            {
                EnableSwingText(PC.GetUIController(), PC.GetPlayerInput());
                if (PC.SwingPressed)
                {
                    PlayerStateMachine.ChangeState(PC.SwingingState);
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
            PC.GetRigidbody().AddForce(Physics.gravity*PC.gravityMultiplier);
            PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
                Space.World);
        }
    }