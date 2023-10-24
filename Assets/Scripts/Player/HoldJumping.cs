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
    private bool isJumping;
    private bool canJump = true; //for dialogue
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
        if (!canJump) return; //for dialogue
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
    }
    void OnJump(InputValue inputValue)
    {
        if (!canJump) return; //for dialogue
        //Gives change in state of the space button
        SpacePressed = Convert.ToBoolean(inputValue.Get<float>());
    }
    public void SetCanJump(bool canJumpValue)
    {
        canJump = canJumpValue;
    }
}
