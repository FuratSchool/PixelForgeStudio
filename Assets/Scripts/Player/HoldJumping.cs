using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HoldJumping : MonoBehaviour
{
    private bool SpacePressed;
    private float distToGround;
    private Rigidbody _rigidbody;
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float force = 10f;
    [SerializeField] private float forceHoldJump = 1f;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float gravityScale = 1f;
    private static float globalGravity = -9.81f;
    
    private bool isFalling;

    private bool isJumping;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }
    //checks if player is grounded using raycasting
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }

    private void FixedUpdate()
    {
        if (IsGrounded() && SpacePressed && isJumping == false)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
        if(isJumping && SpacePressed)
        {
            if (jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(Vector3.up * forceHoldJump, ForceMode.Impulse);
                jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                isJumping = false;
            }
        }
        else
        {
            jumpTimeCounter = 0;
        }

        if (!isJumping && _rigidbody.velocity.y < fallSpeed)
        {
            _rigidbody.useGravity = false;
            isFalling = true;
        }

        if (isFalling)
        {
            if (FindObjectOfType<PlayerMovement>().IsDashing == false)
            {
                Vector3 gravity = globalGravity * gravityScale * Vector3.up;
                _rigidbody.AddForce(gravity, ForceMode.Acceleration);
                if (IsGrounded())
                {
                    isFalling = false;
                    _rigidbody.useGravity = true;
                }
            }
        }
    }
    void OnJump(InputValue inputValue)
    {
        //Gives change in state of the space button
        SpacePressed = Convert.ToBoolean(inputValue.Get<float>());
    }
}
