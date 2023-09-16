using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("movement variables")] [SerializeField]
    private float speed = 7f;


    [Header("Gravity")] [SerializeField] private float _gravity = -9.81f;

    [SerializeField] private float gravityMultiplier = 3.0f;

    [Header("Jumping")] [SerializeField] private float jumpPower = 6f;

    private bool _backward;
    private CharacterController _characterController;
    private Vector3 _direction;
    private bool _forward;
    private Vector3 _rotation;
    private float _velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyGravity();
    }


    private void ApplyMovement()
    {
        if (_forward)
            _characterController.Move(_characterController.transform.forward * (speed * Time.deltaTime));
        else if (_backward)
            _characterController.Move(-_characterController.transform.forward * (speed * Time.deltaTime));
        _characterController.transform.Rotate(_rotation * (100f * Time.deltaTime));
        _characterController.Move(_direction * (speed * Time.deltaTime));
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
            _velocity = -1.0f;
        else
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;

        _direction.y = _velocity;
    }


    public void MoveLinear(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<float>();
        switch (movement)
        {
            case > 0:
                _forward = true;
                _backward = false;
                break;
            case < 0:
                _forward = false;
                _backward = true;
                break;
            default:
                _forward = false;
                _backward = false;
                break;
        }
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        var inp = context.ReadValue<float>();

        _rotation = new Vector3(0, inp, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        _velocity += jumpPower;
    }

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }
}