public class DeathState : IPlayerState
{
    private PlayerController _playerController;
    private PlayerStateMachine _stateMachine;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _playerController = stateMachine.GetPlayerController();
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