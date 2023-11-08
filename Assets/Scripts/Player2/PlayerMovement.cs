using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TrailRenderer tr;
    private Rigidbody _rigidbody;
    public Transform footPrints;
    public float totalTime = 0;

    //sets the speed of the player.

    [Header("Movement")] [SerializeField] private float speed = 10f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float normalSpeed = 10f;
    [SerializeField] private float _footprintOffset = 0f; 
    [Header("Turning")] [SerializeField] private float turnSmoothTime = 0.15f;

    [Header("Dashing")] [SerializeField] private float dashingPower = 30;
    
    
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1f;
    private Camera _camera;
    private bool _canDash = true;
    public bool IsDashing => _isDashing;
    private bool _isDashing;
    private Vector3 _lastDirection;
    private Vector2 _movement = Vector2.zero;
    private bool _isMoving = false;
    public WhiteScreen _whiteScreen;
    private float _turnSmoothVelocity;
    private bool canMove = true;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isDashing || !canMove) return;
        if (_whiteScreen.isTransitioning && _whiteScreen.lockMovement) return;
        PlayerMove();
        if (_isMoving && FindObjectOfType<HoldJumping>().IsGrounded())
        {
            FootPrint();
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movement = inputValue.Get<Vector2>();
    }

    private void OnSprintStart()
    {
        speed = maxSpeed;
    }

    private void OnSprintFinish()
    {
        speed = normalSpeed;
    }

    private void OnDash()
    {
        //if (_canDash) StartCoroutine(Dash());
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
            _isMoving = true;
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
        else
        {
            _isMoving = false;
        }

        return new Vector3();
    }

    /*private IEnumerator Dash()
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
    }*/

    private void FootPrint()
    {
        totalTime += Time.deltaTime;
        if (totalTime > .5f)
        {
            var rotAmount = Quaternion.Euler(90, 0, 0);
            
            if(Physics.Raycast(transform.position, -Vector3.up, out var hit, 0.5f)){
                var posOffset = new Vector3(0f, _footprintOffset, 0f);
                Instantiate(footPrints, hit.point + posOffset, (transform.rotation * rotAmount));
            }
            totalTime = 0;
        }
    }
    
    public void SetCanMove(bool canMoveValue)
    {
        canMove = canMoveValue;
    }
}