public class DeathState : IPlayerState
{
    private PlayerController _playerController;

    public void EnterState(PlayerStateMachine stateMachine)
    {
        _playerController = stateMachine.GetPlayerController();
    }

    public void UpdateState(PlayerStateMachine stateMachine)
    {
        
        var spawnPoint = _playerController.GetComponent<PlayerStatus>().GetSpawnPoint();
        _playerController.transform.position = spawnPoint;

        stateMachine.ChangeState(stateMachine.IdleState);
    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void ExitState(PlayerStateMachine stateMachine)
    {
    }
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
}