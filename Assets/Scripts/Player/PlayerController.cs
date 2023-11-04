using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 12.0f;
    private float moveSpeed;
    public bool ShiftPressed { get; set; }
    public Vector2 Movement { get; set; }
    
    [Header("Turning")] [SerializeField] private float turnSmoothTime = 0.1f;
    private TrailRenderer tr;

    [Header("Jumping")]
    [SerializeField] public float jumpTime = 0.35f;
    [SerializeField] public float jumpTimeCounter;
    [SerializeField] public float force = 10f;
    [SerializeField] public float forceHoldJump = 1f;
    [SerializeField] public float raycastDistance = .2f;
    public bool IsJumping;
    public bool SpacePressed { get; set; }
    public float MaxSpeed { get; set; } = 10f;
    
    [Header("Dashing")]
    [SerializeField] public float dashingTime = 0.35f;
    [SerializeField] public float dashingCooldown = 0.35f;
    [SerializeField] public float dashSpeed = 10f;
    public bool canDash = true;
    public bool DashPressed { get; set; }
    
    [Header("Swinging")]
    [SerializeField] private float swingDistance = 4f;
    [SerializeField] private float exitForce = 4f;
    [SerializeField] private float swingTime = 3f;
    
    public bool SwingPressed { get; set; }
    public bool IsSwinging { get; set; } = false;
    public bool canSwing { get; set; } = true;

    public bool inSwingingRange { get; set; }
    
    public GameObject SwingableObjectGAME { get; set; }
    
    public WhiteScreen _whiteScreen;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;
    private UIController _uiController;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    
    public float SprintSpeed
    {
        get => sprintSpeed;
        set => sprintSpeed = value;
    }
    
    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }
    public float SwingDistance
    {
        get => swingDistance;
        set => swingDistance = value;
    }
    
    public float ExitForce
    {
        get => exitForce;
        set => exitForce = value;
    }
    public float SwingTime
    {
        get => swingTime;
        set => swingTime = value;
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
            return Mathf.Abs(Movement.x) >= movementThreshold || Mathf.Abs(Movement.y) >= movementThreshold;
        }
    }
    private void Awake()
    {
        TR = GetComponent<TrailRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _stateMachine.SetPlayerController(this);
    }

    private void Start()
    {
        _uiController = FindObjectOfType<UIController>();
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
    
    public UIController GetUIController()
    {
        return _uiController;
    }
    
    public bool IsOnTerrain()
    {
        if (transform.position.y < -10f)
            _stateMachine.ChangeState(new DeathState());
        return false;
    }
    
    private void OnMove(InputValue inputValue)
    {
        Movement = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        SpacePressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    
    private void OnSprint(InputValue inputValue)
    {
        ShiftPressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    
    private void OnDash(InputValue inputValue)
    {
        DashPressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    
    private void OnSwing(InputValue inputValue)
    {
        SwingPressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    public void DestroyJoint(ConfigurableJoint joint)
    {
        Destroy(joint);
    }
}