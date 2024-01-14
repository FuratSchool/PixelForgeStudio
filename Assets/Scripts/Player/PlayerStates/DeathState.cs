public class DeathState : IPlayerState
{
    public DeathState (PlayerController pc) : base("DeathState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    public override void EnterState()
    {
        base.EnterState();
        _pc.GetRigidbody().isKinematic = true;
    }

    public override void UpdateState()
    {
        
        var spawnPoint = _pc.GetComponent<PlayerStatus>().GetSpawnPoint();
        _pc.transform.position = spawnPoint;
            _pc.touchedWater = false;
            _pc.DeathCount++;
        _playerStateMachine.ChangeState(_pc.IdleState);
    }
    public override void ExitState()
    {
        base.ExitState();
        _pc.GetRigidbody().isKinematic = false;
    }
}