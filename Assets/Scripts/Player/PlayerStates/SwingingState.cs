using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingingState : PlayerState
{
    public SwingingState (PlayerController pc) : base("SwingingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    private bool hasFinishedInit = false;
    public override void EnterState()
    {
        hasFinishedInit = false;
        base.EnterState();
        PC.source.PlayOneShot(PC._scytheSwingSounds);
        PlayerStateMachine.Animator.SetInteger("State", 5);
        Debug.Log(PC._isSwinging.ToString() + PC.inSwingingRange.ToString() + PC.SwingPressed.ToString() + PC._canSwing.ToString());
        Swing();
        hasFinishedInit = true;
    }
    public override void FixedUpdateState() { }
    public override void UpdateState()
    {
        if(!hasFinishedInit) return;
        base.UpdateState();
        if (!PC._isSwinging || !PC.SwingPressed)
            PlayerStateMachine.ChangeState(PC.FallingState);
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        if (!PC.joint) return;
        if (PC.lr.positionCount != 0)
        {
            PC.lr.SetPosition(0, PC.Hand.transform.position);
            PC.lr.SetPosition(1, PC.Handle.transform.position);
            PC.Scythe.transform.position = PC.SwingableObjectGAME.transform.position;
            PC.Scythe.transform.LookAt(PC.Hand.transform);
            PC.Scythe.transform.localEulerAngles = new Vector3(0, PC.Scythe.transform.localEulerAngles.y, 0);

        }
    }

    public override void ExitState() {
        PC.StopAllCoroutines();
        PC.EndSwing();
        PC.ExitSwing = true;
     }

    private void Swing()
    {
        PC._swingpoint = PC.SwingableObjectGAME.transform.position;
        PC.StartCoroutine(TimedSwing());

        PC.SwingableObjectPos = PC.SwingableObjectGAME.transform.gameObject.transform.position;
        MakeJoint(PC._swingpoint);
        PC.lr.positionCount = 2;
        PC._isSwinging = true;
        PC._canSwing = false;
    }
    
    private void MakeJoint(Vector3 hit)
    {
        PC.joint = PC.player.gameObject.AddComponent<ConfigurableJoint>();
        PC.joint.autoConfigureConnectedAnchor = false;
        //joint.connectedBody = hit.rigidbody;
        PC.joint.connectedAnchor = hit;
        PC.joint.anchor = new Vector3(0,0,0);
        
        PC.joint.xMotion = ConfigurableJointMotion.Limited;
        PC.joint.yMotion = ConfigurableJointMotion.Limited;
        PC.joint.zMotion = ConfigurableJointMotion.Limited;
        var limit = new SoftJointLimit();
        limit.limit = PC.SwingDistance;
        limit.bounciness = 0f;
        limit.contactDistance = 0f;
        PC.joint.linearLimit = limit;
    }
    
    private IEnumerator TimedSwing()
    {
        yield return new WaitForSeconds(PC.SwingTime);
        if (PC.joint != null)
        {
            PC.EndSwing();
        }
    }
}