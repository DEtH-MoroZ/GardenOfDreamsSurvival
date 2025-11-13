using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Input Settings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    public Vector2 GetMoveInput()
    {
        return new Vector2(
            Input.GetAxisRaw(horizontalAxis),
            Input.GetAxisRaw(verticalAxis)
        ).normalized;
    }
}