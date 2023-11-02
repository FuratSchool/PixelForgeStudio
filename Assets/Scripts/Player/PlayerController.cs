using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("MoveSpeed")] [Header("Movement")] [SerializeField]
    private float moveSpeed = 6.0f;

    [SerializeField] private float jumpForce = 12.0f;

    [Header("Turning")] [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Components")] [SerializeField]
    private TrailRenderer tr;

    public WhiteScreen _whiteScreen;
    public PlayerMovementController _playerMovementController;
    public bool IsJumping;
    public bool canDash = true;

    public bool canJump = true;
    public Swinging _swingingComponent;

    private Camera _camera;
    private bool _isGrounded;

    private Collider[] _playerCollider;
    private PlayerStatus _playerStatus;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;
    private PlayerStateObserver _stateObserver;

    public Vector3 LastDirection { get; set; }
    public float MaxSpeed { get; set; } = 10f;

    public float HorizontalInput { get; set; }
    public float VerticalInput { get; set; }
    public bool CanSprint { get; set; } = false;
    
    public bool CanMove { get; set; } = true;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public float TurnSmoothTime
    {
        get => turnSmoothTime;
        set => turnSmoothTime = value;
    }

    public TrailRenderer TR
    {
        get => tr;
        set => tr = value;
    }

    public bool IsPlayerMoving
    {
        get
        {
            var movementThreshold = 0.1f;
            return Mathf.Abs(HorizontalInput) >= movementThreshold || Mathf.Abs(VerticalInput) >= movementThreshold;
        }
    }

    public float TurnSmoothVelocity { get; set; }

    private void Awake()
    {
        TR = GetComponent<TrailRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _playerStatus = GetComponent<PlayerStatus>();
        _stateObserver = GetComponent<PlayerStateObserver>();
        _stateMachine.SetPlayerController(this);
        _playerMovementController = GetComponent<PlayerMovementController>();
        _swingingComponent = GetComponent<Swinging>();

        _playerCollider = GetComponents<Collider>();
        _stateObserver.Subscribe(_stateMachine);
        _stateMachine.ChangeState(new IdleState());
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        IsGrounded();
        _stateMachine.UpdateState();
        IsOnTerrain();
    }

    private void OnDestroy()
    {
        _stateObserver.Unsubscribe(_stateMachine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.2f)
            {
                _isGrounded = true;
                break;
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
        foreach (var contact in collision.contacts)
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.2f)
                return;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public bool IsGrounded()
    {
        //Debug.Log(_isGrounded);
        return _isGrounded;
    }


    public bool IsOnTerrain()
    {
        if (transform.position.y < -10f)
            _stateMachine.ChangeState(new DeathState());
        return false;
    }

    public Vector3 PlayerInput()
    {
        return new Vector3(HorizontalInput, 0f, VerticalInput);
    }

    public void SetCanJump(bool canJumpValue)
    {
        canJump = canJumpValue;
    }
}