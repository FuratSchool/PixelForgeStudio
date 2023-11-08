using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : IPlayerState
{
    public JumpingState(PlayerController pc) : base("JumpingState", pc)
    {
        _pc = (PlayerController)this._playerStateMachine;
    }
    
    public override void EnterState()
    {
        base.EnterState();
        _pc.GetAudio().PlayOneShot(_pc.JumpingSound);
        
        /*if (_pc.ShiftPressed)
        {
            _pc.MoveSpeed = _pc.SprintSpeed;
        }*/
        _playerStateMachine.Animator.Play("Start Jump");
        StartJump();

    }

    public override void UpdateState()
    {
        
        base.UpdateState();
        if (_pc._canDash && _pc.dashPressed) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        if (_pc.GetRigidbody().velocity.y < 0)
            _playerStateMachine.ChangeState(_pc.FallingState);
        if (_pc.SpacePressed && _pc.canDoubleJump && _pc.jumpReleased)
            _playerStateMachine.ChangeState(_pc.DoubleJumpState);


        if (_pc.CheckSwing())
        {
            _pc.EnableSwingText(_pc.GetUIController());
            if (_pc.SwingPressed)
            {
                _playerStateMachine.ChangeState(_pc.SwingingState);
            }
        }
        else
        {
            _pc.DisableSwingText(_pc.GetUIController());
        }
    }
    

    public override void ExitState()
    {
        //_playerStateMachine.Animator.SetBool("IsJumping", false);        
        
        //_playerStateMachine.SwingingState.DisableSwingText(_pc.GetUIController());
    }





    public override void LateUpdateState()
    {
        base.LateUpdateState();
        if (!_pc.canJump) return; //for dialogue
        /*if (_pc.IsGrounded() && _pc.SpacePressed && _pc.isJumping == false)
        {
            _pc.isJumping = true;
            //_pc.SpaceReleased = false;
            _pc.jumpTimeCounter = _pc.jumpTime;
            _pc.GetRigidbody().AddForce(Vector3.up * _pc.force, ForceMode.Impulse);
        }*/
        /*if(_pc.isJumping && _pc.SpacePressed)
        {
            if (_pc.jumpTimeCounter > 0)
            {
                _pc.GetRigidbody().AddForce(Vector3.up * _pc.forceHoldJump, ForceMode.Impulse);
                _pc.jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                _pc.isJumping = false;
                //_playerStateMachine.ChangeState(_pc.FallingState);
            }
        }
        else
        {
            _pc.jumpTimeCounter = 0;
            _pc.isJumping = false;
            
        }*/
        ContinueJump();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);
        

        if (!_pc.SpacePressed)
        {
            //_playerStateMachine.ChangeState(_pc.FallingState);
        }
    }
    private void StartJump()
    {
        _pc.jumpTimeCounter = _pc.jumpTime;
        _pc.GetRigidbody().AddForce(Vector3.up * _pc.force, ForceMode.Impulse);
        _pc.isJumping = true;
    }
    
    private void ContinueJump()
    {
        if (_pc.jumpTimeCounter > 0 && _pc.SpacePressed)
        {
            _pc.GetRigidbody().AddForce(Vector3.up * _pc.forceHoldJump, ForceMode.Impulse);
            _pc.jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            _pc.isJumping = false;
        }
    }
}