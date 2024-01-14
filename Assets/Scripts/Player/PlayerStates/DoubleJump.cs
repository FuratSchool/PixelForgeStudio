using UnityEngine;

public class DoubleJump : PlayerState
{
    private bool _grounded;
    private float _distToGround;
    public DoubleJump(PlayerController pc) : base("DoubleJump", pc) {PC = (PlayerController)this.PlayerStateMachine;}


    public override void EnterState()
    {
        base.EnterState();
        PC.GetRigidbody().velocity = new Vector3(PC.GetRigidbody().velocity.x, 0f , PC.GetRigidbody().velocity.z);
        PC.GetRigidbody().AddForce(Vector3.up * (PC.force * 1f), ForceMode.Impulse); PC.canDoubleJump = false;
        PC.jumpReleased = false;
        PlayerStateMachine.Animator.SetInteger("State", 10);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (PC.GetRigidbody().velocity.y < 0)
        {
            PlayerStateMachine.ChangeState(PC.FallingState);
        }
        else if (PC._canDash && PC.dashPressed) 
            PlayerStateMachine.ChangeState(PC.DashingState);
        
    }

    public override void LateUpdateState()
    {
        base.LateUpdateState();
        PC.GetRigidbody().transform.Translate(PC.GetDirection(PC.PlayerInput()).normalized * ((PC.MoveSpeed * PC.speedBoostMultiplier) * Time.deltaTime), 
            Space.World);
    }
}