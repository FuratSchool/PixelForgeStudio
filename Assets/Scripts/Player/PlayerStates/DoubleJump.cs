using UnityEngine;

public class DoubleJump : IPlayerState
{
    

    private bool _grounded;
    private float distToGround;
    

    public DoubleJump(PlayerController pc) : base("DoubleJump", pc) {_pc = (PlayerController)this._playerStateMachine;}


    public override void EnterState()
    {
        base.EnterState();
        _pc.GetRigidbody().velocity = new Vector3(_pc.GetRigidbody().velocity.x, 0f , _pc.GetRigidbody().velocity.z);
        _pc.GetRigidbody().AddForce(Vector3.up * (_pc.force * 1.5f), ForceMode.Impulse); _pc.canDoubleJump = false;
        _pc.jumpReleased = false;
        //_playerStateMachine.Animator.Play("DoubleJump");
        _playerStateMachine.Animator.SetInteger("State", 10);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_pc.GetRigidbody().velocity.y < 0)
        {
            _playerStateMachine.ChangeState(_pc.FallingState);
        }
        if (_pc._canDash && _pc.dashPressed) 
            _playerStateMachine.ChangeState(_pc.DashingState);
        
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        _pc.GetRigidbody().transform.Translate(_pc.GetDirection(_pc.PlayerInput()).normalized * (_pc.MoveSpeed * Time.deltaTime), 
            Space.World);
    }
}