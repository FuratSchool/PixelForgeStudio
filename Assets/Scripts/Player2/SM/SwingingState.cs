using System.Collections;
using UnityEngine;

public class SwingingState2 : BaseState
{
    
    

    public SwingingState2 (MovementSM stateMachine) : base("SwingingState", stateMachine) {_sm = (MovementSM)this.stateMachine;}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Swinging");
    }
    

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (_sm._isSwinging == false && _sm.InRange)
        {
            CheckSwing();
        }
        if (_sm._dialueActive && !_sm.InRange)
        {
            _sm._UIController.SetInteractableTextActive(false);
            _sm._dialueActive = false;
        }
        if (_sm.SwingPressed == false && _sm._isSwinging)
        {
            _sm.EndSwing();
        }
        
        
        if (!_sm._isSwinging)
            stateMachine.ChangeState(_sm.fallingState);
    }

    public override void UpdatePhysics()
    {
        if (!_sm.joint) return;
        if (_sm.lr.positionCount != 0)
        {
            _sm.lr.SetPosition(0, _sm.player.transform.GetChild(1).transform.position);
            _sm.lr.SetPosition(1, _sm.SwingableObjectPos); 
        }
    }
    
    private void CheckSwing()
    {
        if (_sm._canSwing)
        {
            _sm._UIController.SetInteractText("Hold E to Swing");
            _sm._dialueActive = true;
            _sm._UIController.SetInteractableTextActive(true);
            _sm.collision = _sm.SwingableObjectGAME.transform.position;
            if(_sm.SwingPressed)
            {
                _sm.StartCoroutine(TimedSwing());

                _sm.SwingableObjectPos = _sm.SwingableObjectGAME.transform.gameObject.transform.position;
                MakeJoint(_sm.collision);
                _sm.lr.positionCount = 2;
                _sm._isSwinging = true;
                _sm._canSwing = false;
            }
        }
    }
    
    private void MakeJoint(Vector3 hit)
    {
        _sm.joint = _sm.player.gameObject.AddComponent<ConfigurableJoint>();
        _sm.joint.autoConfigureConnectedAnchor = false;
        //joint.connectedBody = hit.rigidbody;
        _sm.joint.connectedAnchor = hit;
        _sm.joint.anchor = new Vector3(0,0,0);
        
        _sm.joint.xMotion = ConfigurableJointMotion.Limited;
        _sm.joint.yMotion = ConfigurableJointMotion.Limited;
        _sm.joint.zMotion = ConfigurableJointMotion.Limited;
        var limit = new SoftJointLimit();
        limit.limit = _sm.SwingDistance;
        limit.bounciness = 0f;
        limit.contactDistance = 0f;
        _sm.joint.linearLimit = limit;
    }
    
    private IEnumerator TimedSwing()
    {
        yield return new WaitForSeconds(_sm.SwingTime);
        if (_sm.joint != null)
        {
            _sm.EndSwing();
        }
        
        
    }
    

}