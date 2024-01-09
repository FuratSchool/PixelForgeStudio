using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : IPlayerState
{
    public JumpingState(PlayerController pc) : base("JumpingState", pc)
    {
        _pc = (PlayerController)this._playerStateMachine;
    }
    
    private bool _uiActive = false;
    private bool _jumpOver = false;
    public override void EnterState()
    {
        base.EnterState();
        _jumpOver = false;
        _pc.jumped = true;
        _pc.isJumping = true;
        _pc.GetAudio().PlayOneShot(_pc.JumpingSound);
        _playerStateMachine.Animator.SetInteger("State", 3);
        StartJump();

    }

    public override void UpdateState()
    {
        
        base.UpdateState();
        if (_pc._canDash && _pc.dashPressed) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        // if (_pc.GetRigidbody().velocity.y < -0)
        //     _playerStateMachine.ChangeState(_pc.FallingState);
        if (_pc.SpacePressed && _pc.canDoubleJump && _pc.jumpReleased)
            _playerStateMachine.ChangeState(_pc.DoubleJumpState);


        if (CheckSwing())
        {
            _uiActive = true;
            EnableSwingText(_pc.GetUIController(), _pc.GetPlayerInput());
            if (_pc.SwingPressed)
            {
                _playerStateMachine.ChangeState(_pc.SwingingState);
            }
        }
        else
        {
            if(_uiActive)
            {
                DisableSwingText(_pc.GetUIController());
                _uiActive = false;
            }
        }
    }
    
    public override void ExitState()
    {
        _pc.canJump = false;
    }
    
    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);
        if (!_pc.canJump) return; //for dialogue
        
        ContinueJump();
        
        if (!_pc.SpacePressed)
        {
            _pc.CanJumpAgain = true;
        }
        else
        {
            _pc.CanJumpAgain = false;
        }
    }
    private void StartJump()
    {
        _pc.jumpTimeCounter = _pc.jumpTime;
        _pc.GetRigidbody().AddForce(Vector3.up * _pc.force, ForceMode.Impulse);
        //_pc.isJumping = true;
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
            //_pc.isJumping = false;
            _pc.canJump = false;
            _playerStateMachine.ChangeState(_pc.FallingState);
            _jumpOver = true;
        }
    }
}