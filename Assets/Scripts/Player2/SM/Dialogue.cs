using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueState2 : BaseState
{
    public DialogueState2 (MovementSM stateMachine) : base("DialogueState", stateMachine) {_sm = (MovementSM)this.stateMachine;}
    
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered dialogue state");
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(_sm.canMove)
            stateMachine.ChangeState(_sm.idleState);
    }
}
