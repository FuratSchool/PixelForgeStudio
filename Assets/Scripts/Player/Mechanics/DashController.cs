using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController
{
    private readonly Vector3 _lastDirection;
    private readonly PlayerMovementController _playerMovement;
    private readonly Rigidbody _rigidbody;
    private readonly float dashingCooldown;
    private readonly float dashingPower;
    private readonly float dashingTime;
    private readonly TrailRenderer tr;
    private bool _canDash;
    private bool _isDashing;

    public DashController(
        bool canDash,
        bool isDashing,
        Rigidbody rigidbody,
        TrailRenderer transform,
        Vector3 lastDirection,
        float power,
        float time,
        float cooldown,
        PlayerMovementController playerMovement)

    {
        _canDash = canDash;
        _isDashing = isDashing;
        _rigidbody = rigidbody;
        tr = transform;
        _lastDirection = lastDirection;
        dashingPower = power;
        dashingTime = time;
        dashingCooldown = cooldown;
        _playerMovement = playerMovement;
    }

    public IEnumerator Dash()
    {
        PrepareForDash();
        PerformDash();
        FinishDash();
        yield return new WaitForSeconds(dashingCooldown);
        EnableDashing();
    }

    private void PrepareForDash()
    {
        _canDash = false;
        _isDashing = true;
        _rigidbody.useGravity = false;
    }

    private IEnumerable<WaitForSeconds> PerformDash()
    {
        _rigidbody.velocity = _playerMovement.GetDirection(_lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
    }

    private void FinishDash()
    {
        _rigidbody.useGravity = true;
        _rigidbody.velocity = Vector3.zero;
        _isDashing = false;
    }

    private void EnableDashing()
    {
        _canDash = true;
    }
}