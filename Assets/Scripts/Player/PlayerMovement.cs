using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //sets the speed of the player.
    [SerializeField] private float speed = 6f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    private Rigidbody _rigidbody;
    private float turnSmoothVelocity;
    private Vector2 movement = Vector2.zero;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void PlayerMove(Vector3 direction)
    {
        
        //checks if the player is moving.
        if(direction.magnitude  >= 0.1f)
        {
            //calculates the angle of the direction the player is moving.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg+ Camera.main.transform.eulerAngles.y;
            //it smooths the smooths the transistion between the transform.eulerAngles.y and the targetAngle.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
            //sets the rotation of the player to the angle.
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            //rotates the player to the direction they are moving.
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //moves the player. taking into account the delta time, world space, and the speed.
            transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
        }
    }
    void Update()
    {
        Vector3 direction = new Vector3(x: movement.x, y: 0, z: movement.y);
        PlayerMove(direction);        
    }

    void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }
}