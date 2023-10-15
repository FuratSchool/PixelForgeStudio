using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    private bool _isJumping;
    private float HorizontalInput;


    private bool isGrounded; // Add this flag

    private Collider[] playerCollider;
    private PlayerStateMachine stateMachine;
    private PlayerStateObserver stateObserver;

    private float VerticalInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = new PlayerStateMachine();
        stateObserver = new PlayerStateObserver();
        playerCollider = GetComponents<Collider>();
        stateObserver.Subscribe(stateMachine);
        stateMachine.ChangeState(new IdleState());
    }


    private void Update()
    {
        stateMachine.UpdateState();

        HorizontalInput = GetHorizontalInput();
        VerticalInput = GetVerticalInput();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the observer to avoid memory leaks
        stateObserver.Unsubscribe(stateMachine);
    }


    // Other methods...

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true; // Player is grounded
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false; // Player is no longer grounded
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public Transform GetTransform()
    {
        return transform; // This returns the Transform component of the player object.
    }

    public Collider[] GetColliders()
    {
        return playerCollider;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsJumping()
    {
        return _isJumping;
    }

    public float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    public float GetVerticalInput()
    {
        return Input.GetAxis("Vertical");
    }

    public void SetIsJumping(bool IsJumping)
    {
        _isJumping = IsJumping;
    }
}