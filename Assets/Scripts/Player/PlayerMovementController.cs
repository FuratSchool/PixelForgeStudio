using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Turning")] [SerializeField] private float turnSmoothTime = 0.1f;

    public float TurnSmoothVelocity;
    public bool canDash = true;
    public bool canJump = true;
    public bool _canSwing = true;
    private Camera _camera;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;
    public Vector3 _lastDirection;
    public Vector2 _movement = Vector2.zero;

    private float _turnSmoothVelocity;
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
        PlayerMove();
    }

    public void OnMove()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
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
        if (FindObjectOfType<SwingingController>().IsSwinging)
        {
            var velocity = _rigidbody.velocity;
            if (velocity.magnitude < _playerController.MaxSpeed)
            {
                _rigidbody.AddForce(GetDirection(PlayerInput()).normalized * (_playerController.MoveSpeed * Time.deltaTime),ForceMode.VelocityChange);
            }
        }
        else
        {
            transform.Translate(GetDirection(PlayerInput()).normalized * (_playerController.MoveSpeed * Time.deltaTime), 
                Space.World);
        }
        
        
    }

    public Vector3 GetDirection(Vector3 direction)
    {
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
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);
                //sets the rotation of the player to the angle.
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //rotates the player to the direction they are moving.
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }
        }

        return new Vector3();
    }
    
    public Vector3 PlayerInput()
    {
        var direction = new Vector3(_movement.x, 0, _movement.y);
        return direction;
    }
}