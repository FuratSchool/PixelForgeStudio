using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TrailRenderer tr;
    private Rigidbody _rigidbody;

    //sets the speed of the player.

    [Header("Movement")] [SerializeField] private float speed = 6f;
    [SerializeField] private float maxSpeed = 10f;
    [Header("Turning")] [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Dashing")] [SerializeField] private float dashingPower = 12f;
    
    
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1f;
    private Camera _camera;
    private bool _canDash = true;
    private bool _isDashing;
    private Vector3 _lastDirection;
    private Vector2 _movement = Vector2.zero;
    
    private float _turnSmoothVelocity;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isDashing) return;
        PlayerMove();
    }

    private void OnMove(InputValue inputValue)
    {
        _movement = inputValue.Get<Vector2>();
    }

    private void OnSprintStart()
    {
        speed = 12f;
    }

    private void OnSprintFinish()
    {
        speed = 6f;
    }

    private void OnDash()
    {
        if (_canDash) StartCoroutine(Dash());
    }

    private void PlayerMove()
    {
        //moves the player. taking into account the delta time, world space, and the speed.
        if (FindObjectOfType<Swinging>().IsSwinging)
        {
            var velocity = _rigidbody.velocity;
            if (velocity.magnitude < maxSpeed)
            {
                _rigidbody.AddForce(GetDirection(PlayerInput()).normalized * (speed * Time.deltaTime),ForceMode.VelocityChange);
            }
        }
        else
        {
            transform.Translate(GetDirection(PlayerInput()).normalized * (speed * Time.deltaTime), 
                Space.World);
        }
        
        
    }

    private Vector3 PlayerInput()
    {
        var direction = new Vector3(_movement.x, 0, _movement.y);
        return direction;
    }

    private Vector3 GetDirection(Vector3 direction)
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

    private IEnumerator Dash()
    {
        //var direction = lastDirection;
        _canDash = false;
        _isDashing = true;
        _rigidbody.useGravity = false;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _rigidbody.velocity = GetDirection(_lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }
}