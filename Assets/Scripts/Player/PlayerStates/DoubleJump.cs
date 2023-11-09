using UnityEngine;

public class DoubleJump : IPlayerState
{
    

    private bool _grounded;
    private float distToGround;
    

    public DoubleJump(PlayerController pc) : base("DoubleJump", pc) {_pc = (PlayerController)this._playerStateMachine;}


    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("DoubleJump");
        _pc.GetRigidbody().AddForce(Vector3.up * (_pc.force * 2f), ForceMode.Impulse);
        _pc.canDoubleJump = false;
        _pc.jumpReleased = false;
        _playerStateMachine.Animator.Play("Start Jump");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _playerStateMachine.ChangeState(_pc.FallingState);
    }
    

}