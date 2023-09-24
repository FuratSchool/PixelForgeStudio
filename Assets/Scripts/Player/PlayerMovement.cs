
using System;
using System.Collections;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //sets the speed of the player.
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float dashingPower = 12f;
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    private Camera _camera;
    private bool _canDash = true;
    private bool _isDashing;
    private Vector3 _lastDirection;

    private Rigidbody _rigidbody;

    private float _turnSmoothVelocity;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }


    

    private void Update()
    {
        if (_isDashing) return;
        CheckDash();
        CheckSprint();
        PlayerMove(); 
    }

    Vector3 PlayerInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }

    void PlayerMove()
    {
        //moves the player. taking into account the delta time, world space, and the speed.
            transform.Translate(GetDirection(PlayerInput()).normalized * (speed * Time.deltaTime), Space.World);
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
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                    _camera.transform.eulerAngles.y;
                //it smooths the transition between the transform.eulerAngles.y and the targetAngle.
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);
                //sets the rotation of the player to the angle.
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //rotates the player to the direction they are moving.
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                return moveDir;
            }
        }

        return new Vector3();
    }

    private void CheckSprint()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? 12f : 6f;
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown("q") && _canDash)
        {
            StartCoroutine(Dash());
        }
        
    }

    private IEnumerator Dash()
    {
        //var direction = lastDirection;
        _canDash = false;
        Debug.Log("dash false");
        _isDashing = true;
        _rigidbody.useGravity = false;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _rigidbody.velocity = GetDirection(_lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = new Vector3(0f,0f,0f);
        _isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        Debug.Log("dash true");
        _canDash = true;
    }
}