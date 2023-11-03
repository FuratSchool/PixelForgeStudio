using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField] private float dashingCooldown = 2.0f;
    [SerializeField] private float dashingPower = 10.0f;
    [SerializeField] private float dashingTime = 1.0f;

    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private Rigidbody _rigidbody;

    public void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerMovement = FindObjectOfType<PlayerMovementController>();
        _rigidbody = _playerController.GetRigidbody();
    }

    public IEnumerator Dash()
    {
        Debug.Log("Starting Dash");

        PrepareForDash();
        yield return StartCoroutine(PerformDash());
        FinishDash();
        Debug.Log("Finished Dash");

        yield return new WaitForSeconds(dashingCooldown);
        EnableDashing();
        Debug.Log("Can Dash");

        _playerMovement.IsDashing = false;

    }

    private void PrepareForDash()
    {
        _playerMovement.canDash = false;
        _playerMovement.IsDashing = true;
        _playerController.CanMove = false;
        _playerMovement.canJump = false;
        _rigidbody.useGravity = false;
    }

    private IEnumerator PerformDash()
    {
        _rigidbody.velocity = _playerMovement.GetDirection(_playerMovement._lastDirection).normalized * dashingPower;
        _playerController.TR.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _playerController.TR.emitting = false;
    }

    private void FinishDash()
    {
        _rigidbody.useGravity = true;
        _rigidbody.velocity = Vector3.zero;
        _playerMovement.IsDashing = false;
        _playerController.CanMove = true;
        _playerMovement.canJump = true;

    }

    private void EnableDashing()
    {
        _playerMovement.canDash = true;
    }
}