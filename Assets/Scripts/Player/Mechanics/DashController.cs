using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField] private readonly float dashingCooldown = 2.0f;

    [SerializeField] private readonly float dashingPower = 10.0f;

    [SerializeField] private readonly float dashingTime = 1.0f;
    private bool _isDashing;
    private PlayerController _playerController;
    private PlayerMovementController _playerMovement;
    private Rigidbody _rigidbody;
    private PlayerStateMachine _stateMachine;

    public void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _stateMachine = FindObjectOfType<PlayerStateMachine>();
        _playerMovement = FindObjectOfType<PlayerMovementController>();
        _rigidbody = _playerController.GetRigidbody();
    }

    public IEnumerator Dash()
    {
        PrepareForDash();
        yield return StartCoroutine(PerformDash());
        FinishDash();
        yield return new WaitForSeconds(dashingCooldown);
        EnableDashing();
    }

    private void PrepareForDash()
    {
        _playerController.canDash = false;
        _isDashing = true;
        _rigidbody.useGravity = false;
    }

    private IEnumerator PerformDash()
    {
        _rigidbody.velocity = _playerMovement.GetDirection().normalized * dashingPower;
        _playerController.TR.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _playerController.TR.emitting = false;
    }

    private void FinishDash()
    {
        _rigidbody.useGravity = true;
        _rigidbody.velocity = Vector3.zero;
        _isDashing = false;
    }

    private void EnableDashing()
    {
        _playerController.canDash = true;
    }
}