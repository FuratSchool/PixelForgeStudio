using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerStateMachine
{
    private Camera _camera;
    private TrailRenderer tr;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 15.0f;
    private float moveSpeed;
    public Vector2 Movement = Vector2.zero;
    public Transform footPrints;
    public float totalTime = 0;
    public bool _isRunning;
    
    [Header("Jumping")]
    [SerializeField] public float jumpTime = 0.35f;
    [SerializeField] public float jumpTimeCounter;
    [SerializeField] public float force = 10f;
    [SerializeField] public float forceHoldJump = 1f;
    [SerializeField] public float raycastDistance = .4f;
    [SerializeField] public float gravityMultiplier = 1.0f;
    public bool SpacePressed { get; set; }
    public bool canJump;
    public bool canDoubleJump;
    public bool jumpReleased = false;
    public bool grounded;
    
    [Header("Dashing")]
    [SerializeField] public float dashingTime = 0.5f;
    [SerializeField] public float dashingCooldown = 2f;
    [SerializeField] private float dashingPower = 30f;
    [SerializeField] private float dashEndTime = .35f;
    public bool isDashing;
    //private Vector3 _lastDirection;
    private Vector3 _dashDirection;

    [Header("Turning")] 
    [SerializeField] private float turnSmoothTime = 0.15f;
    private float _turnSmoothVelocity;
    
    [Header("Swinging")]
    [SerializeField] private float swingDistance = 4f;
    [SerializeField] private float exitForce = 15f;
    [SerializeField] private float swingTime = 3f;
    [SerializeField] private float SwingDelay = 1f;
    [SerializeField] public LineRenderer lr;
    [SerializeField] public GameObject Hand;
    public ConfigurableJoint joint;
    public GameObject player;
    public Vector3 SwingableObjectPos;
    public bool _isSwinging;
    public Vector3 _swingpoint;
    public bool SwingPressed { get; set; }
    public bool IsSwinging { get; set; } = false;
    public bool canSwing { get; set; } = true;
    public bool inSwingingRange { get; set; }
    public GameObject SwingableObjectGAME { get; set; }
    
    public bool InteractPressed { get; set; }
    public bool InDialogeTriggerZone { get; set; }
    public bool DialogueActive { get; set; }
    public DialogueTrigger NPC { get; set; }
    
    [Header("footprints")]
    public float footstepIntervalWalking = 0.3f;
    public float footstepIntervalRunning = 0.15f;

    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;
    private UIController _uiController;
    private DialogueManager _dialogueManager;
    private PlayerInput _playerInput;
    public AudioClip WalkingSound;
    public AudioClip RunningSound;
    public AudioClip JumpingSound;
    public AudioClip LandingSound;
    
    public bool isTransitioning = false;
    
    public bool _canDash = true;
    public bool dashPressed;
    public bool _canSwing = true;
    public bool InRange { get; set; }

    [HideInInspector] public WalkingState WalkingState;
    [HideInInspector] public IdleState IdleState;
    [HideInInspector] public DashingState DashingState;
    [HideInInspector] public JumpingState JumpingState;
    [HideInInspector] public FallingState FallingState;
    [HideInInspector] public SwingingState SwingingState;
    [HideInInspector] public SprintingState SprintingState;
    [HideInInspector] public TalkingState TalkingState;
    [HideInInspector] public TransitionState TransitionState;
    [HideInInspector] public DeathState DeathState;
    [HideInInspector] public DoubleJump DoubleJumpState;
    
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
    
    public bool isJumping { get; set; }

    private void Awake()
    {
        WalkingState = new WalkingState(this);
        IdleState = new IdleState(this);
        DashingState = new DashingState(this);
        JumpingState = new JumpingState(this);
        SwingingState = new SwingingState(this);
        SprintingState = new SprintingState(this);
        TalkingState = new TalkingState(this);
        TransitionState = new TransitionState(this);
        DeathState = new DeathState(this);
        FallingState = new FallingState(this);
        DoubleJumpState = new DoubleJump(this);
        _uiController = FindObjectOfType<UIController>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        TR = GetComponent<TrailRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _camera = Camera.main;
        GetDirection(PlayerInput());
    }
    
    protected override IPlayerState GetInitialState()
    {
        return IdleState;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
    
    public UIController GetUIController()
    {
        return _uiController;
    }
    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }
    public DialogueManager GetDialogueManager()
    {
        return _dialogueManager;
    }
    
    public AudioSource GetAudio()
    {
        return GetComponent<AudioSource>();
    }

    private void OnMove(InputValue inputValue)
    {
        Movement = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        SpacePressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    private void OnJumpReleased()
    {
        jumpReleased = true;
    }
    
    private void OnSprintStart() { _isRunning = true; }

    private void OnSprintFinish() { _isRunning = false; }
    
    private void OnDash(InputValue inputValue)
    {
        dashPressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    
    private void OnSwing(InputValue inputValue)
    {
        SwingPressed = Convert.ToBoolean(inputValue.Get<float>());
    }

    public void OnInteract(InputValue inputValue)
    {
        InteractPressed = Convert.ToBoolean(inputValue.Get<float>());
    }
   
    public bool IsGrounded()
    {
        var layermask = 1 << 6;
        bool ground = Physics.Raycast(transform.position, Vector3.down,out var hit, raycastDistance, ~layermask);
        /*if (hit.collider != null)
        {
            if (hit.collider.name != "FirstCliff") Debug.Log("hit");
        }*/
        
        return ground;
    }
    
    public Vector3 GetDirection(Vector3 direction)
    {
        //checks if the player is moving.
        if (direction.magnitude >= 0.1f)
        {
            //_lastDirection = direction;
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
                
                _dashDirection =Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                
                //rotates the player to the direction they are moving.
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }
        }

        return new Vector3();
    }
    public Vector3 PlayerInput()
    {
        var direction = new Vector3(Movement.x, 0, Movement.y);
        return direction;
    }
    
    public IEnumerator Dash()
    {
        _canDash = false;
        isDashing = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = _dashDirection * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        Animator.Play("Stop Dash");
        yield return new WaitForSeconds(dashEndTime);
        tr.emitting = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }
    
    public void FootPrint(float interfall)
    {
        totalTime += Time.deltaTime;
        if (totalTime > interfall)
        {
            var rotAmount = Quaternion.Euler(90, 0, 0);
            var posOffset = new Vector3(0f, 0f, 0f);
            Instantiate(footPrints, (transform.position + posOffset), (transform.rotation * rotAmount));
            totalTime = 0;
        }
    }
    
    public void EndSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
        _isSwinging = false;
        player.GetComponent<Rigidbody>().AddForce(ExitForce * Vector3.up, ForceMode.Impulse);
        StartCoroutine(SwingDelayTimer());
    }
    private IEnumerator SwingDelayTimer()
    {
        yield return new WaitForSeconds(SwingDelay);
        _canSwing = true;
    }
    
    

    
    
    
}