using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PhycisMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float force = 10;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool IsGrounded()
    {
        return GetComponent<Rigidbody>().velocity.y == 0;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("space") && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
