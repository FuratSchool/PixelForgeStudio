using UnityEngine;

public class JumpingController : MonoBehaviour
{
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float force = 10f;
    [SerializeField] private float forceHoldJump = 1f;
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _playerMovement = GetComponent<PlayerMovementController>();
        _stateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    private void FixedUpdate()
    {
        if (CanPerformJump()) return;
        {
            if (!_playerController.canJump) return;
        }
        if (CanJump()) StartJump();

        if (IsJumping())
            ContinueJump();
        else
            jumpTimeCounter = 0;
    }

    private bool CanPerformJump()
    {
        return !_playerController.canJump;
    }

    private bool CanJump()
    {
        return _playerController.IsGrounded() && _playerMovement.SpacePressed && !_playerController.IsJumping;
    }

    private void StartJump()
    {
        _playerController.IsJumping = true;
        jumpTimeCounter = jumpTime;
        _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    private bool IsJumping()
    {
        return _playerController.IsJumping && _playerMovement.SpacePressed;
    }

    private void ContinueJump()
    {
        if (jumpTimeCounter > 0)
        {
            _rigidbody.AddForce(Vector3.up * forceHoldJump, ForceMode.Impulse);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            _playerController.IsJumping = false;
        }
    }
}