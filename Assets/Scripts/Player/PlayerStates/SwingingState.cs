using System.Collections;
using UnityEngine;

public class SwingingState : IPlayerState
{
    public SwingingState (PlayerController pc) : base("SwingingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    public override void EnterState()
    {
        base.EnterState();
        _playerStateMachine.Animator.Play("Swinging"); 
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
        
        /*if (_pc._dialueActive && !_pc.InRange)
        {
            _pc._dialueActive = false;
        }*/
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
            _pc.lr.SetPosition(0, _pc.player.transform.GetChild(1).transform.position);
            _pc.lr.SetPosition(1, _pc.SwingableObjectPos); 
        }
    }

    public override void ExitState()
    {

    }

    private void Swing()
    {
        if (_pc._canSwing)
        {
            //_pc._UIController.SetInteractText("Hold E to Swing");
            //_pc._dialueActive = true;
            //_pc._UIController.SetInteractableTextActive(true);
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