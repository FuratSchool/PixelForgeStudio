public class DeathState : IPlayerState
{
    public DeathState (PlayerController pc) : base("DeathState", pc) {_pc = (PlayerController)this._playerStateMachine;}

    public override void UpdateState()
    {
        
        var spawnPoint = _pc.GetComponent<PlayerStatus>().GetSpawnPoint();
        _pc.transform.position = spawnPoint;
            _pc.touchedWater = false;
        _playerStateMachine.ChangeState(_pc.IdleState);
    }
    
}