using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PhycisMovement : MonoBehaviour
{
    private float distToGround;
    private Rigidbody _rigidbody;
    private bool spaceispressed;
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float force = 10;

    private bool isJumping;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded()
    {
         return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) spaceispressed = true;
    }
    

    private void FixedUpdate()
    {
        if (spaceispressed)
        {
            spaceispressed = false;
            isJumping = true;
            jumpTimeCounter = jumpTime;
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
        if(Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
                jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
}
