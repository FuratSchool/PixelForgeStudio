using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public float TurnSmoothVelocity;
    public bool canDash = true;
    public bool canJump = true;
    public bool _canSwing = true;
    private Camera _camera;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;
    public bool IsDashing { get; set; }

    private DashController dashController;
    public bool SpacePressed { get; set; }

    public bool SwingPressed { get; set; }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _stateMachine.SetPlayerMovementController(this);
        _rigidbody = _playerController.GetRigidbody();
        _camera = Camera.main;
        IsDashing = false;
    }

    private void Update()
    {
        if (IsDashing || !_playerController.CanMove) return;
        if (_playerController._whiteScreen.isTransitioning && _playerController._whiteScreen.lockMovement) return;
    }

    public void OnMove()
    {
        if (_playerController.CanMove) PlayerMove();
    }

    private void OnJump(InputValue inputValue)
    {
        if (canJump) 
            SpacePressed = Convert.ToBoolean(inputValue.Get<float>());

    }

    private void OnDash()
    {
        var dashController = GetComponent<DashController>();

        if (canDash && !IsDashing && _stateMachine.GetCurrentState() is not IdleState)
        {
            IsDashing = true;
            StartCoroutine(dashController.Dash());
        }
    }


    public void OnSprintStart()
    {
        _stateMachine.ChangeState(new SprintingState());
    }

    public void OnSprintFinish()
    {
        _playerController.MoveSpeed = 6f;
    }

    private void OnSwing(InputValue input)
    {
        SwingPressed = Convert.ToBoolean(input.Get<float>());
    }

    public void PlayerMove()
    {
        //moves the player. taking into account the delta time, world space, and the speed.
        if (FindObjectOfType<Swinging>().IsSwinging)
        {
            var velocity = _rigidbody.velocity;
            if (velocity.magnitude < _playerController.MaxSpeed)
                _rigidbody.AddForce(GetDirection().normalized * (-_playerController.MoveSpeed * Time.deltaTime),
                    ForceMode.VelocityChange);
        }
        else
        {
            transform.Translate(GetDirection().normalized * (_playerController.MoveSpeed * Time.deltaTime),
                Space.World);
        }
    }

    public Vector3 GetDirection()
    {
        var direction = _playerController.PlayerInput();
        //checks if the player is moving.
        if (direction.magnitude >= 0.1f)
            //calculates the angle of the direction the player is moving.
            if (_camera != null)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                  _camera.transform.eulerAngles.y;
                //it smooths the transition between the transform.eulerAngles.y and the targetAngle.
                var playerControllerTurnSmoothVelocity = _playerController.TurnSmoothVelocity;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                    ref playerControllerTurnSmoothVelocity,
                    _playerController.TurnSmoothTime);
                //sets the rotation of the player to the angle.ß
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //rotates the player to the direction they are moving.
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }

        return new Vector3();
    }
}