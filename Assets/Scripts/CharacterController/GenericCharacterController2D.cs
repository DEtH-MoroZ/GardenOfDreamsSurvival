using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputHandler))]
public class GenericCharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Physics")]
    public bool useForceMovement = false;

    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

    void FixedUpdate()
    {
        if (useForceMovement)
        {
            MoveWithForce();
        }
        else
        {
            MoveDirect();
        }
    }

    private void MoveDirect()
    {
        Vector2 targetVelocity = _moveInput * moveSpeed;
        _rb.velocity = targetVelocity;
    }

    private void MoveWithForce()
    {
        Vector2 targetVelocity = _moveInput * moveSpeed;
        Vector2 velocityChange = (targetVelocity - _rb.velocity);

        if (_moveInput.magnitude > 0.1f)
        {
            velocityChange *= acceleration;
        }
        else
        {
            velocityChange *= deceleration;
        }

        _rb.AddForce(velocityChange * _rb.mass);
    }

    public void Teleport(Vector2 position)
    {
        _rb.position = position;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }
}