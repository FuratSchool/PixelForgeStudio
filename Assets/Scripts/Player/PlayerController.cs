using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController : PlayerStateMachine
{
    private Camera _camera;
    
    private BoxCollider m_Collider;
    private RaycastHit _hit;
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 15.0f;
    private float moveSpeed;
    public Vector2 Movement = Vector2.zero;
    public Transform footPrintsRight;
    public Transform footPrintsLeft;
    private bool leftFootActive;
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
    public bool jumped;
    public bool jumpReleased = false;
    public bool grounded;
    
    private IEnumerator SpeedboostCoroutine;
    public VisualEffect visualEffect;
    private Color _visualEffectBaseColor;
    public bool SpeedboostActive { get; set; }
    public float SpeedBoostMultiplier = 1f;
    
    [Header("Dashing")]
    [SerializeField] private TrailRenderer tr;
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
    
    public string InteractableText { get; } = " To Interact";
    public string DialogueText { get; } = " To Talk";
    public string SwingText { get; } = " To Swing";
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
    public int DeathCount { get; set; }
    
    public bool InteractableRange { get; set; }
    public bool InteractPressed { get; set; }
    public bool KeyDebounced { get; set; } = true;
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
    
    public bool touchedWater { get; set; }

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
    [HideInInspector] public LandingState LandingState;
    [HideInInspector] public InteractingState InteractingState;
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
        LandingState = new LandingState(this);
        InteractingState = new InteractingState(this);
        _uiController = FindObjectOfType<UIController>();
        _dialogueManager = FindObjectOfType<DialogueManager>();
        //TR = GetComponent<TrailRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _camera = Camera.main;
        GetDirection(PlayerInput());
        m_Collider = GetComponent<BoxCollider>();
        Application.targetFrameRate = 120;
        visualEffect = FindObjectOfType<VisualEffect>();
        _visualEffectBaseColor = visualEffect.GetVector4("Color");
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
        //bool ground = Physics.Raycast(transform.position, Vector3.down,out var hit, raycastDistance, ~layermask);
        bool ground = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, -transform.up,out _hit,transform.rotation, raycastDistance , ~layermask );
        
        /*if (_hit.collider != null)
        {
            if (_hit.collider.name != "FirstCliff") Debug.Log("hit");
        }*/
        
        return ground;
    }
    
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (IsGrounded())
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(m_Collider.bounds.center, -transform.up * _hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(m_Collider.bounds.center + -transform.up * _hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(m_Collider.bounds.center, -transform.up * raycastDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(m_Collider.bounds.center + -transform.up * raycastDistance, transform.localScale);
        }
    }*/
    
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
        //Animator.Play("Stop Dash");
        Animator.SetInteger("State", 8);
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
        if (!(totalTime > interfall)) return;
        var rotAmount = Quaternion.Euler(90, 0, 0);
        if (leftFootActive)
        {
            var footPrint = Instantiate(footPrintsLeft, (transform.position), (transform.rotation * rotAmount));
            footPrint.transform.SetParent(transform);
            footPrint.transform.localPosition = new Vector3(-.5f,0,0);
            footPrint.transform.SetParent(null);
            leftFootActive = false;

        }
        else
        {
            var footPrint = Instantiate(footPrintsRight, (transform.position), (transform.rotation * rotAmount));
            footPrint.transform.SetParent(transform);
            footPrint.transform.localPosition = new Vector3(.5f,0,0);
            footPrint.transform.SetParent(null);
            leftFootActive = true;
        }
            
        totalTime = 0;
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
    
    public IEnumerator KeyDebounce()
    {
        KeyDebounced = false;
        yield return new WaitForSeconds(0.25f);
        KeyDebounced = true;
    }

    public void StartSpeedBoost(float boost, float duration, Color color)
    {
        if(SpeedboostActive) StopCoroutine(SpeedboostCoroutine);
        SpeedboostCoroutine = ESpeedBoost(boost, duration,color);
        StartCoroutine(SpeedboostCoroutine);
        
    }

    private IEnumerator ESpeedBoost(float boost, float duration,Color color)
    {
        SpeedboostActive = true;
        SpeedBoostMultiplier = boost;
        visualEffect.SetVector4("Color",color);
        yield return new WaitForSeconds(duration);
        visualEffect.SetVector4("Color",_visualEffectBaseColor);
        SpeedBoostMultiplier = 1f;
        SpeedboostActive = false;
    }

    public void EnableGrimParticles(bool enable)
    {
        if (enable)
        {
            visualEffect.SetFloat("FlameRate", 50f);
            visualEffect.SetFloat("SmokeRate", 2f);
            visualEffect.SetFloat("AmberRate", 16f);
        }
        else
        {
            visualEffect.SetFloat("FlameRate", 0f);
            visualEffect.SetFloat("SmokeRate", 0f);
            visualEffect.SetFloat("AmberRate", 0f);
        }
    }
}