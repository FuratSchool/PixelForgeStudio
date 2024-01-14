using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingState : PlayerState
{
    public InteractingState (PlayerController pc) : base("InteractingState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    public override void EnterState()
    {
        PC.KeyDebounced = false;
        PC.StartCoroutine(PC.KeyDebounce());
        var objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var obj in objects)
        {
            obj.SendMessage("InInteractState", PC, SendMessageOptions.DontRequireReceiver);
        }
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (PC.InteractPressed && PC.KeyDebounced)
        {
            PlayerStateMachine.ChangeState(PC.IdleState);
        }
    }
    
    public override void ExitState()
    {
        PC.StartCoroutine(PC.KeyDebounce());
        var objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var obj in objects)
        {
            obj.SendMessage("InInteractState", PC, SendMessageOptions.DontRequireReceiver);
        }
    }
    public override void LateUpdateState() { }
}
