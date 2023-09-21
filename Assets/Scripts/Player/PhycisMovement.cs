using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PhycisMovement : MonoBehaviour
{
    private float distToGround;
    private Rigidbody _rigidbody;

    [SerializeField] private float force = 10;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded()
    {
         return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    
    void OnJump(InputValue inputValue)
    {
        if (IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
