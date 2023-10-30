using UnityEngine;

public class JumpingController
{
    private readonly PlayerController _playerController;
    private readonly Rigidbody _rigidbody;
    [SerializeField] private readonly float force = 10f;
    [SerializeField] private readonly float forceHoldJump = 1f;
    [SerializeField] private readonly float jumpTime = 0.35f;
    private float jumpTimeCounter;

    public JumpingController(PlayerController playerController)
    {
        _playerController = playerController;
        _rigidbody = _playerController.GetRigidbody();
    }

    public void Jump()
    {
        if (!_playerController.CanJump) return; //for dialogue
        if (_playerController.IsGrounded() && _playerController.SpacePressed && _playerController.IsJumping == false)
        {
            _playerController.IsJumping = true;
            jumpTimeCounter = jumpTime;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        if (_playerController.IsJumping && _playerController.SpacePressed)
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
        else
        {
            jumpTimeCounter = 0;
        }
    }

    public void OnJump()
    {
        if (!_playerController.CanJump) return;
        _playerController.SpacePressed = true;
        Jump();
    }
}