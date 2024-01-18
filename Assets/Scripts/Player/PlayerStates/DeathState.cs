public class DeathState : PlayerState
{
    public DeathState (PlayerController pc) : base("DeathState", pc) {PC = (PlayerController)this.PlayerStateMachine;}

    public override void EnterState()
    {
        base.EnterState();
        PC.GetRigidbody().isKinematic = true;
    }

    public override void UpdateState()
    {
        
        var spawnPoint = PC.GetComponent<PlayerStatus>().GetSpawnPoint();
        PC.transform.position = spawnPoint;
            PC.touchedWater = false;
            PC.DeathCount++;
        PlayerStateMachine.ChangeState(PC.IdleState);
    }
    public override void ExitState()
    {
        base.ExitState();
        PC.GetRigidbody().isKinematic = false;
    }
}