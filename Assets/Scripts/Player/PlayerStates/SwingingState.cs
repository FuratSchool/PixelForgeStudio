using System.Collections;
using UnityEngine;

public class SwingingState : IPlayerState
{
    private PlayerController _playerController;
    private LineRenderer _lr;
    private Vector3 _swingPoint;
    private ConfigurableJoint _joint;
    private bool _endSwinging = false;
    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
        _lr = _playerController.GetComponent<LineRenderer>();
        _playerController.IsSwinging = true;
        _swingPoint = _playerController.SwingableObjectGAME.transform.position;
        _lr.positionCount = 2;
        MakeJoint(_swingPoint);
        stateMachine.Animator.SetBool("IsSwinging", true);        

    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        stateMachine.WalkingState.PlayerMove(stateMachine);
        if(!_playerController.SwingPressed || _endSwinging)
            stateMachine.ChangeState(stateMachine.FallingState);
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        EndSwing();
        stateMachine.Animator.SetBool("IsSwinging", false);        

    }

    public bool CheckSwing(PlayerStateMachine stateMachine)
    {
        var controller = stateMachine.GetPlayerController();
        if(!controller.IsSwinging && controller.canSwing && controller.inSwingingRange)
            return true;
        return false;
    }
    
    public void EnableSwingText(UIController uiController)
    {
        uiController.SetInteractText("Hold [E][Y] to swing");
        uiController.SetInteractableTextActive(true);
    }
    
    public void DisableSwingText(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
    
    private void MakeJoint(Vector3 swingPoint)
    {
        _joint = _playerController.gameObject.AddComponent<ConfigurableJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = swingPoint;
        _joint.anchor = new Vector3(0,0,0);
        
        _joint.xMotion = ConfigurableJointMotion.Limited;
        _joint.yMotion = ConfigurableJointMotion.Limited;
        _joint.zMotion = ConfigurableJointMotion.Limited;
        var limit = new SoftJointLimit();
        limit.limit = _playerController.SwingDistance;
        limit.bounciness = 0f;
        limit.contactDistance = 0f;
        _joint.linearLimit = limit;
    }
    private void EndSwing()
    {
        _lr.positionCount = 0;
        _playerController.DestroyJoint(_joint);
        _playerController.IsSwinging = false;
        _playerController.GetRigidbody().AddForce(_playerController.ExitForce * Vector3.up, ForceMode.Impulse);
    }
    
    private IEnumerator TimedSwing()
    {
        yield return new WaitForSeconds(_playerController.SwingTime);
        if (_joint != null)
            _endSwinging = true;
            
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {
        if (!_joint) return;
        if (_lr.positionCount != 0)
        {
            _lr.SetPosition(0, _playerController.transform.GetChild(1).transform.position);
            _lr.SetPosition(1, _swingPoint); 
        }
    }
}