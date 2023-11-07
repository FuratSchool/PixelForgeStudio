using UnityEngine;

public class WalkingState : IPlayerState
{
    private PlayerController _playerController;
    
    private Camera _camera;
    public Vector3 _lastDirection;
    private Rigidbody _rigidbody;
    private float TurnSmoothVelocity;
    private PlayerStateMachine _stateMachine;
    private float _turnSmoothVelocity;
    private float turnSmoothTime = 0.15f;
    private Rigidbody _rb;
    private float totalTime;
    public void EnterState(PlayerStateMachine stateMachine)
    {
        
        _stateMachine = stateMachine;
        _playerController = stateMachine.GetPlayerController();
        _playerController.GetAudio().clip = _playerController.WalkingSound;
        _playerController.GetAudio().Play();
        _rb = _playerController.GetRigidbody();
        _camera = Camera.main;
        _playerController.MoveSpeed = _playerController.WalkSpeed;
        _playerController.footstepInterval = _playerController.footstepIntervalWalking;
        stateMachine.Animator.SetBool("IsWalking", true);        

    }
    public void FixedUpdateState(PlayerStateMachine stateMachine)
    {
    }
    public void UpdateState(PlayerStateMachine stateMachine)
    {
        PlayerMove(stateMachine);
        if (!stateMachine.GetPlayerController().IsPlayerMoving)
            stateMachine.ChangeState(stateMachine.IdleState);
        else if (stateMachine.JumpingState.IsGrounded(stateMachine) && _playerController.SpacePressed)
            stateMachine.ChangeState(stateMachine.JumpingState);
        else if (_playerController.ShiftPressed)
            stateMachine.ChangeState(stateMachine.SprintingState);
        else if (_playerController.canDash && _playerController.DashPressed)
            stateMachine.ChangeState(stateMachine.DashingState);
        if (_playerController.InDialogeTriggerZone)
        {
            stateMachine.TalkingState.EnableInteractDialogueActive(_playerController.GetUIController());
            if (_playerController.InteractPressed)
            {
                stateMachine.ChangeState(stateMachine.TalkingState);
            }
        }
        else
        {
            stateMachine.TalkingState.DisableInteractDialogueActive(_playerController.GetUIController());
        }
    }

    public void ExitState(PlayerStateMachine stateMachine)
    {
        stateMachine.Animator.SetBool("IsWalking", false);        
        _playerController.GetAudio().Stop();
        // Cleanup or transition logic, if necessary
        stateMachine.TalkingState.DisableInteractDialogueActive(_playerController.GetUIController());

    }
    
    public void PlayerMove(PlayerStateMachine stateMachine)
    {
        if(stateMachine.GetPlayerController().FootprintEnabled)FootPrint(stateMachine);
        var playerControl = stateMachine.GetPlayerController();
        //moves the player. taking into account the delta time, world space, and the speed.
        if (stateMachine.CurrentState is SwingingState)
        {
            var velocity = playerControl.GetRigidbody().velocity;
            if (velocity.magnitude < playerControl.MaxSpeed)
            {
                playerControl.GetRigidbody().AddForce(GetDirection(PlayerInput(stateMachine),stateMachine).normalized * (playerControl.MoveSpeed * Time.deltaTime),ForceMode.VelocityChange);
            }
        }
        else
        {
            stateMachine.transform.Translate(GetDirection(PlayerInput(stateMachine),stateMachine).normalized * (playerControl.MoveSpeed * Time.deltaTime), 
                Space.World);
        }
        
        
    }

    public Vector3 GetDirection(Vector3 direction, PlayerStateMachine state)
    {
        var playerControl = state.GetPlayerController();
        //checks if the player is moving.
        if (direction.magnitude >= 0.1f)
        {
            _lastDirection = direction;
            //calculates the angle of the direction the player is moving.
            if (_camera != null)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                  _camera.transform.eulerAngles.y;
                //it smooths the transition between the transform.eulerAngles.y and the targetAngle.
                var angle = Mathf.SmoothDampAngle(playerControl.transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity,
                    turnSmoothTime);
                //sets the rotation of the player to the angle.
                playerControl.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //rotates the player to the direction they are moving.
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }
        }

        return new Vector3();
    }
    
    public Vector3 PlayerInput(PlayerStateMachine stateMachine)
    {
        var _movement = stateMachine.GetPlayerController().Movement;
        var direction = new Vector3(_movement.x, 0, _movement.y);
        return direction;
    }
    
    public void LateUpdateState(PlayerStateMachine stateMachine)
    {}
    
    private void FootPrint(PlayerStateMachine stateMachine)
    {
        var PlayerControl = stateMachine.GetPlayerController();
        totalTime += Time.deltaTime;
        if (totalTime > PlayerControl.footstepInterval)
        {
            var rotAmount = Quaternion.Euler(90, 0, 0);
            
            if(Physics.Raycast(PlayerControl.transform.position, -Vector3.up, out var hit, 0.5f, ~LayerMask.GetMask("Player"))){
                var posOffset = new Vector3(0f, PlayerControl.FootprintOffset, 0f);
                PlayerControl.InstantiateObject(PlayerControl.FootprintPrefab, hit.point + posOffset, (PlayerControl.transform.rotation * rotAmount));
            }
            totalTime = 0;
        }
    }
}