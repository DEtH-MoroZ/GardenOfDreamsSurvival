using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GenericCharacterController2D _controller;
    public IInputProvider _inputProvider;

    void Awake()
    {
        _controller = GetComponent<GenericCharacterController2D>();
        if (_inputProvider == null )
        _inputProvider = GetComponent<IInputProvider>();
        if (_inputProvider == null)
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        _controller.SetMoveInput(_inputProvider.GetMoveInput());        
    }
}