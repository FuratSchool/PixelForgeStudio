using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingState : IPlayerState
{
    public InteractingState (PlayerController pc) : base("InteractingState", pc) {_pc = (PlayerController)this._playerStateMachine;}
    
    public override void EnterState()
    {
        _pc.KeyDebounced = false;
        _pc.StartCoroutine(_pc.KeyDebounce());
        var objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var obj in objects)
        {
            obj.SendMessage("InInteractState", _pc, SendMessageOptions.DontRequireReceiver);
        }
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (_pc.InteractPressed && _pc.KeyDebounced)
        {
            
            _playerStateMachine.ChangeState(_pc.IdleState);
        }
    }
    
    public override void ExitState()
    {
        _pc.StartCoroutine(_pc.KeyDebounce());
        var objects = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (var obj in objects)
        {
            obj.SendMessage("InInteractState", _pc, SendMessageOptions.DontRequireReceiver);
        }
    }
    public override void LateUpdateState()
    {
    }
}
