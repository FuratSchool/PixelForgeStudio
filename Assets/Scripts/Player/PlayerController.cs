using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [SerializeField] public float raycastDistance = .8f;
    public bool SpacePressed { get; set; }
    public bool canJump = true;
    public bool canDoubleJump;
    public bool jumpReleased = false;
    
    [Header("Dashing")]
    [SerializeField] public float dashingTime = 0.5f;
    [SerializeField] public float dashingCooldown = 2f;
    [SerializeField] private float dashingPower = 30f;
    public bool isDashing;
    private Vector3 _lastDirection;

    [Header("Turning")] 
    [SerializeField] private float turnSmoothTime = 0.15f;
    private float _turnSmoothVelocity;
    
    [Header("Swinging")]
    [SerializeField] private float swingDistance = 4f;
    [SerializeField] private float exitForce = 15f;
    [SerializeField] private float swingTime = 3f;
    [SerializeField] private float SwingDelay = 1f;
    [SerializeField] public LineRenderer lr;
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
    
    public bool IsPlayerMoving
    {
        get
        {
            var movementThreshold = 0.1f;
            return Mathf.Abs(Movement.x) >= movementThreshold || Mathf.Abs(Movement.y) >= movementThreshold;
        }
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
    
    public DialogueManager GetDialogueManager()
    {
        return _dialogueManager;
    }
    
    public AudioSource GetAudio()
    {
        return GetComponent<AudioSource>();
    }
    /*public bool IsOnTerrain()
    {
        if (transform.position.y < -36f)
            _stateMachine.ChangeState(_stateMachine.DeathState);
        return false;
    }
    public bool IsTransitioning()
    {
        if (isTransitioning && _stateMachine.CurrentState != _stateMachine.TransitionState)
            _stateMachine.ChangeState(_stateMachine.TransitionState);
        return false;
    }*/
    
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
    public void DestroyJoint(ConfigurableJoint joint)
    {
        Destroy(joint);
    }
    
    public void InstantiateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(prefab, position, rotation);
    }
    
    public bool IsGrounded()
    {
        var layermask = 1 << 6;
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance, ~layermask);
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
        var direction = new Vector3(Movement.x, 0, Movement.y);
        return direction;
    }
    
    public IEnumerator Dash()
    {
        //var direction = lastDirection;
        _canDash = false;
        isDashing = true;
        _rigidbody.useGravity = false;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _rigidbody.velocity = GetDirection(_lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        Animator.Play("Stop Dash");
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
            ;           Instantiate(footPrints, (transform.position + posOffset), (transform.rotation * rotAmount));
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
    
    public void EnableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractText("Press [F][X] to talk");
        uiController.SetInteractableTextActive(true);
    }
    
    public void DisableInteractDialogueActive(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }

    public bool CheckSwing()
    {
        if(!IsSwinging && canSwing && inSwingingRange)
            return true;
        return false;
    }
    public void EnableSwingText(UIController uiController)
    {
        uiController.SetInteractText("Hold [E][Y] to swing");
        uiController.SetInteractableTextActive(true);
    }
    
    public void DisableSwingText(UIController uiController)
    {
        uiController.SetInteractableTextActive(false);
    }
    
}