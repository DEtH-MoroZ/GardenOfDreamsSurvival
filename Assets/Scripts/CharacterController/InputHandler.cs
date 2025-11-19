using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GenericCharacterController2D _controller;
    public IMovementInputProvider _movementInputProvider;


    void Awake()
    {
        _controller = GetComponent<GenericCharacterController2D>();
        if (_movementInputProvider == null )
        _movementInputProvider = GetComponent<IMovementInputProvider>();
        if (_movementInputProvider == null)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        _controller.SetMoveInput(_movementInputProvider.GetMoveInput());        
    }
}