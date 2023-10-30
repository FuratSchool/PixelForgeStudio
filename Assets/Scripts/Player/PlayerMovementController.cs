using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float TurnSmoothVelocity;
    private Camera _camera;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _stateMachine.SetPlayerMovementController(this);
        _rigidbody = _playerController.GetRigidbody();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_playerController.IsDashing || !_playerController.CanMove) return;
        if (_playerController._whiteScreen.isTransitioning && _playerController._whiteScreen.lockMovement) return;
    }

    public void OnMove()
    {
        PlayerMove();
    }

    private void OnDash()
    {
        if (_playerController.CanDash) _stateMachine.ChangeState(new DashingState());
    }

    public void OnSprintStart()
    {
        _stateMachine.ChangeState(new SprintingState());
    }

    public void OnSprintFinish()
    {
        _playerController.MoveSpeed = 6f;
    }

    public void PlayerMove()
    {
        // if (FindObjectOfType<Swinging>().IsSwinging)
        // {
        //     var velocity = _rigidbody.velocity;
        //     if (velocity.magnitude < _playerController.MaxSpeed)
        //         _rigidbody.AddForce(
        //             GetDirection(_playerController.PlayerInput()).normalized *
        //             (_playerController.MoveSpeed * Time.deltaTime),
        //             ForceMode.VelocityChange);
        // }
        // else
        // {
        //     transform.Translate(
        //         GetDirection(_playerController.PlayerInput()).normalized *
        //         (_playerController.MoveSpeed * Time.deltaTime),
        //         Space.World);
        // }
        transform.Translate(
            GetDirection(_playerController.PlayerInput()).normalized *
            (_playerController.MoveSpeed * Time.deltaTime),
            Space.World);
    }

    public Vector3 GetDirection(Vector3 playerInput)
    {
        var direction = _playerController.PlayerInput();
        if (_playerController.PlayerInput().magnitude >= 0.1f)
        {
            _playerController.LastDirection = _playerController.PlayerInput();
            //calculates the angle of the direction the player is moving.
            if (_camera != null)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                  _camera.transform.eulerAngles.y;
                //it smooths the transition between the transform.eulerAngles.y and the targetAngle.
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                    ref TurnSmoothVelocity,
                    _playerController.TurnSmoothTime);
                //sets the rotation of the player to the angle.
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //rotates the player to the direction they are moving.
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }
        }

        return new Vector3();
    }
}