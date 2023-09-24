using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //sets the speed of the player.
    [SerializeField] private float speed = 6f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    [SerializeField] private float gravityMultiplier = 3.0f;

    private float turnSmoothVelocity;

    private Rigidbody _rigidbody;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.5f;
    private float dashingCooldown = 1f;
    private Vector3 lastDirection;
    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    Vector3 PlayerInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        return direction;
    }
    void PlayerMove(Vector3 direction)
    {
        
            
            //moves the player. taking into account the delta time, world space, and the speed.
            transform.Translate(GetDirection(PlayerInput()).normalized * (speed * Time.deltaTime), Space.World);
        
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        //checks if the player is moving.
        if (direction.magnitude >= 0.1f)
        {
            lastDirection = direction;
            //calculates the angle of the direction the player is moving.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                Camera.main.transform.eulerAngles.y;
            //it smooths the smooths the transistion between the transform.eulerAngles.y and the targetAngle.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                TurnSmoothTime);
            //sets the rotation of the player to the angle.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //rotates the player to the direction they are moving.
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            return moveDir;
        }

        return new Vector3();
    }
    void Update()
    {
              
    }
    
    private void FixedUpdate()
    {
        if (isDashing) return;
            CheckDash();
            CheckSprint();
            PlayerMove(PlayerInput());  
        }

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 12f;
            
        }
        else
        {
            speed = 6f;
           
        }
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown("q") && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        //var direction = lastDirection;
        canDash = false;
        isDashing = true;
        _rigidbody.useGravity = false;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
        //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _rigidbody.velocity = GetDirection(lastDirection).normalized * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = new Vector3(0f,0f,0f);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}