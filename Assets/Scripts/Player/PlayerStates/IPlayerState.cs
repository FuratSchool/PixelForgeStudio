public interface IPlayerState
{
    void EnterState(PlayerStateMachine stateMachine);
    void UpdateState(PlayerStateMachine stateMachine);
    void FixedUpdateState(PlayerStateMachine stateMachine);
    void LateUpdateState(PlayerStateMachine stateMachine);
    void ExitState(PlayerStateMachine stateMachine);
}