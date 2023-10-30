public class DeathState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerStateMachine stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        _playerController = this.stateMachine.GetPlayerController();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        var spawnPoint = PlayerStatus.playerStatus.GetSpawnPoint();
        _playerController.transform.position = spawnPoint;

        stateMachine.ChangeState(new IdleState());
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
}