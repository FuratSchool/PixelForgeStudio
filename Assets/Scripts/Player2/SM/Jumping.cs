using UnityEngine;

public class Jumping : BaseState
{
    

    private bool _grounded;
    private float distToGround;
    

    public Jumping(MovementSM stateMachine) : base("Jumping", stateMachine)
    {
        _sm = (MovementSM)this.stateMachine;
    }
    
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Jumping");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (_sm._canDash && _sm._dashPressed) 
            stateMachine.ChangeState(_sm.dashingState);
        if (_sm.SwingPressed && _sm._canSwing && _sm.InRange)
            stateMachine.ChangeState(_sm.swingingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.grounded = _sm.IsGrounded();
        if (!_sm.canJump) return; //for dialogue
        if (_sm.IsGrounded() && _sm.SpacePressed && _sm.isJumping == false)
        {
            _sm.isJumping = true;
            //_sm.SpaceReleased = false;
            _sm.jumpTimeCounter = _sm.jumpTime;
            _sm.rigidbody.AddForce(Vector3.up * _sm.force, ForceMode.Impulse);
        }
        if(_sm.isJumping && _sm.SpacePressed)
        {
            if (_sm.jumpTimeCounter > 0)
            {
                _sm.rigidbody.AddForce(Vector3.up * _sm.forceHoldJump, ForceMode.Impulse);
                _sm.jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                _sm.isJumping = false;
                stateMachine.ChangeState(_sm.fallingState);
            }
        }
        else
        {
            _sm.jumpTimeCounter = 0;
            _sm.isJumping = false;
            
        }
        _sm.rigidbody.transform.Translate(_sm.GetDirection(_sm.PlayerInput()).normalized * (_sm.speed * Time.deltaTime), 
            Space.World);
        

        if (!_sm.SpacePressed)
        {
            stateMachine.ChangeState(_sm.fallingState);
        }

    }

}