using UnityEngine;

public class EmoteState : PlayerState
{
    public EmoteState (PlayerController pc) : base("EmoteState", pc) {PC = (PlayerController)this.PlayerStateMachine;}
    
    private GameObject _chair;
    public override void EnterState()
    {
        PC.EnableGrimParticles(false);
        PlayerStateMachine.Animator.SetInteger("State", 20);
        PC.WaitOver = false;
        _chair = PC.InstantiateFunc(PC.chair, GameObject.Find("ChairSpawnPoint").transform.position, Quaternion.identity);
        _chair.transform.Rotate((_chair.transform.rotation.x-90),PC.transform.localRotation.eulerAngles.y,0);
        PC.WaitSecs(5);
        base.EnterState();
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if(PC.WaitOver)
            PlayerStateMachine.ChangeState(PC.IdleState);
    }

    public override void ExitState()
    {
        PC.DestroyFunc(_chair);
        PC.EnableGrimParticles(true);
    }
}