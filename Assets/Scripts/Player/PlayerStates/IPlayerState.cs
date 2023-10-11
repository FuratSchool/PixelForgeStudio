public interface IPlayerState
{
    void EnterState(PlayerStateMachine stateMachine);
    void UpdateState(PlayerStateMachine stateMachine);
    void ExitState(PlayerStateMachine stateMachine);
}