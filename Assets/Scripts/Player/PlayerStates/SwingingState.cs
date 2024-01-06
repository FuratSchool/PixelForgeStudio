using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingingState : IPlayerState
{
    public SwingingState (PlayerController pc) : base("SwingingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        base.EnterState();
        //_playerStateMachine.Animator.Play("Swinging");
        _playerStateMachine.Animator.SetInteger("State", 5);
        if (!_pc._isSwinging && _pc.inSwingingRange)
        {
            Swing();
        }

    }
    public override void FixedUpdateState()
    {
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (!_pc.SwingPressed && _pc._isSwinging)
        {
            _pc.EndSwing();
        }
        
        
        if (!_pc._isSwinging)
            _playerStateMachine.ChangeState(_pc.FallingState);
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        if (!_pc.joint) return;
        if (_pc.lr.positionCount != 0)
        {
            _pc.lr.SetPosition(0, _pc.Hand.transform.position);
            _pc.lr.SetPosition(1, _pc.SwingableObjectPos); 
            Vector3 targetPostition = new Vector3( _pc.Scythe.transform.position.x,_pc.SwingableObjectGAME.transform.position.y
                , _pc.SwingableObjectGAME.transform.position.z ) ;
            _pc.Scythe.transform.LookAt(targetPostition);
            _pc.Scythe.transform.Rotate(0,0,-90);
            _pc.Scythe.transform.localEulerAngles = new Vector3(0, _pc.Scythe.transform.localEulerAngles.y, 0);
        }
    }

    public override void ExitState()
    {

    }

    private void Swing()
    {
        if (_pc._canSwing)
        {
            _pc._swingpoint = _pc.SwingableObjectGAME.transform.position;
            if(_pc.SwingPressed)
            {
                _pc.StartCoroutine(TimedSwing());

                _pc.SwingableObjectPos = _pc.SwingableObjectGAME.transform.gameObject.transform.position;
                MakeJoint(_pc._swingpoint);
                _pc.lr.positionCount = 2;
                _pc._isSwinging = true;
                _pc._canSwing = false;
            }
        }
    }
    
    private void MakeJoint(Vector3 hit)
    {
        _pc.joint = _pc.player.gameObject.AddComponent<ConfigurableJoint>();
        _pc.joint.autoConfigureConnectedAnchor = false;
        //joint.connectedBody = hit.rigidbody;
        _pc.joint.connectedAnchor = hit;
        _pc.joint.anchor = new Vector3(0,0,0);
        
        _pc.joint.xMotion = ConfigurableJointMotion.Limited;
        _pc.joint.yMotion = ConfigurableJointMotion.Limited;
        _pc.joint.zMotion = ConfigurableJointMotion.Limited;
        var limit = new SoftJointLimit();
        limit.limit = _pc.SwingDistance;
        limit.bounciness = 0f;
        limit.contactDistance = 0f;
        _pc.joint.linearLimit = limit;
    }
    
    private IEnumerator TimedSwing()
    {
        yield return new WaitForSeconds(_pc.SwingTime);
        if (_pc.joint != null)
        {
            _pc.EndSwing();
        }
        
        
    }
}