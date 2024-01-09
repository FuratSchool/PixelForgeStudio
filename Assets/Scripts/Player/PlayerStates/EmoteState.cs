using UnityEngine;

public class EmoteState : IPlayerState
{
    public EmoteState (PlayerController pc) : base("EmoteState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    private bool _textActive = false;
    private GameObject chair;
    public override void EnterState()
    {
        _pc.EnableGrimParticles(false);
        _playerStateMachine.Animator.SetInteger("State", 20);
        _pc.waitOver = false;
        chair = _pc.InstantiateFunc(_pc.Chair, GameObject.Find("ChairSpawnPoint").transform.position, Quaternion.identity);
        chair.transform.Rotate((chair.transform.rotation.x-90),_pc.transform.localRotation.eulerAngles.y,0);
        _pc.WaitSecs(5);
        base.EnterState();
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if(_pc.waitOver)
            _playerStateMachine.ChangeState(_pc.IdleState);
    }

    public override void ExitState()
    {
        _pc.DestroyFunc(chair);
        _pc.EnableGrimParticles(true);
    }

    public override void LateUpdateState()
    {
    }
}