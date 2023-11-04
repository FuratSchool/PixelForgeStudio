using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class HoldJumping : MonoBehaviour
{
    private bool SpacePressed;
    private Rigidbody _rigidbody;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float force = 5f;
    [SerializeField] private float forceHoldJump = 1f;
    [SerializeField] private bool grounded = false;
    private bool isJumping;
    private bool SpaceReleased = true;
    private bool canJump = true; //for dialogue

    [SerializeField] private float raycastDistance = .05f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //distToGround = GetComponent<Collider>().bounds.extents.y;
    }
    //checks if player is grounded using raycasting
    bool IsGrounded()
    {
        int layermask = 1 << 6;
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance, ~layermask); 
    }
    

    private void FixedUpdate()
    {
        if (!canJump) return; //for dialogue
        if (IsGrounded() && SpacePressed && isJumping == false && SpaceReleased == true)
        {
            Debug.Log("Jump");
            isJumping = true;
            SpaceReleased = false;
            jumpTimeCounter = jumpTime;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
            //Debug.Log(_rigidbody.velocity.y);
        }
        if(isJumping && SpacePressed)
        {
            if (jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(Vector3.up * forceHoldJump, ForceMode.Impulse);
                //Debug.Log(_rigidbody.velocity.y);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else
        {
            //Debug.Log(_rigidbody.velocity.y);
            jumpTimeCounter = 0;
        }

        if (SpacePressed == false)
        {
            SpaceReleased = true;
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
