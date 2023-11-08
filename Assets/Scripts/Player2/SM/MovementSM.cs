using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementSM : StateMachine
{
    
    [Header("Components")] 
    [SerializeField] private TrailRenderer tr;
    private Camera _camera;
    public Rigidbody rigidbody;
    public WhiteScreen _whiteScreen;
    
    [Header("Movement")] 
    [SerializeField] public float speed = 10f;
    [SerializeField] public float maxSpeed = 15f;
    [SerializeField] public float normalSpeed = 10f;
    public bool _isRunning;
    public Vector2 _movement = Vector2.zero;
    public bool canMove = true;
    public Transform footPrints;
    public float totalTime = 0;
    
    [Header("Turning")] 
    [SerializeField] private float turnSmoothTime = 0.15f;
    private float _turnSmoothVelocity;

    [Header("Dashing")] 
    [SerializeField] private float dashingPower = 30f;
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1f;
    public bool _canDash = true;
    public bool _isDashing;
    public bool _dashPressed;
    private Vector3 _lastDirection;
    
    [Header("Jumping")]
    [SerializeField] public float jumpTime = 0.35f;
    [SerializeField] public float jumpTimeCounter;
    [SerializeField] public float force = 10f;
    [SerializeField] public float forceHoldJump = .1f;
    [SerializeField] private float raycastDistance = .8f;
    public bool isJumping;
    public bool SpacePressed;
    public bool JumpPressed;
    public bool canJump = true; //for dialogue
    public bool canDoubleJump;
    public bool jumpReleased = false;
    public bool grounded;
    
    [Header("Swinging")]
    [SerializeField] public float SwingDistance = 7f;
    [SerializeField] private float ExitForce = 15f;
    [SerializeField] public float SwingTime = 3f;
    [SerializeField] private float SwingDelay = 1f;
    [SerializeField] public LineRenderer lr;
    public bool SwingPressed = false;
    public bool _canSwing = true;
    public Vector3 collision;
    public bool _isSwinging;
    public UIController _UIController;
    public bool _dialueActive = false;
    public bool InRange { get; set; }
    public GameObject SwingableObjectGAME { get; set; }
    public ConfigurableJoint joint;
    public Vector3 SwingableObjectPos;
    public GameObject player;
    

    [HideInInspector] public Dashing dashingState;
    [HideInInspector] public Falling fallingState;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Jumping jumpingState;
    [HideInInspector] public Running runningState;
    [HideInInspector] public Walking walkingState;
    [HideInInspector] public SwingingState2 swingingState;
    [HideInInspector] public DialogueState2 dialogueState;
    [HideInInspector] public DoubleJumping doubleJumpingState;

    private void Awake()
    {
        idleState = new Idle(this);
        walkingState = new Walking(this);
        jumpingState = new Jumping(this);
        runningState = new Running(this);
        fallingState = new Falling(this);
        dashingState = new Dashing(this);
        swingingState = new SwingingState2(this);
        dialogueState = new DialogueState2(this);
        doubleJumpingState = new DoubleJumping(this);
        _camera = Camera.main;
        GetDirection(PlayerInput());
        _UIController = FindObjectOfType<UIController>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    private void OnMove(InputValue inputValue)
    {
        _movement = inputValue.Get<Vector2>();
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

    private void OnSprintStart() { _isRunning = true; }

    private void OnSprintFinish() { _isRunning = false; }

    public bool IsGrounded()
    {
        var layermask = 1 << 6;
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance, ~layermask);
    }

    private void OnJump(InputValue inputValue)
    {
        if (!canJump) return; //for dialogue
        //Gives change in state of the space button
        SpacePressed = Convert.ToBoolean(inputValue.Get<float>());
    }

    private void OnJumpReleased()
    {
        jumpReleased = true;
    }

    public void SetCanJump(bool canJumpValue) { canJump = canJumpValue; }

    private void OnDash() { _dashPressed = true; }

    public IEnumerator Dash()
    {
        //var direction = lastDirection;
        _canDash = false;
        _isDashing = true;
        rigidbody.useGravity = false;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        rigidbody.velocity = GetDirection(_lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigidbody.useGravity = true;
        rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    public void SetCanMove(bool canMoveValue) { canMove = canMoveValue; }
    
    void OnSwing(InputValue input) { SwingPressed = Convert.ToBoolean(input.Get<float>()); }
    
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
}