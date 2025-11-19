using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviourExtBind, IMovementInputProvider
{
    [Header("Input Settings")]
    [Header("Movement")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Inventory")]
    public KeyCode InventoryKeyCode = KeyCode.Tab;

    [OnStart]
    public void TheStart()
    {
        Model.Set("InventoryKeyCodeReleased", Input.GetKey(InventoryKeyCode));
    }

    public Vector2 GetMoveInput()
    {
        return new Vector2(
            Input.GetAxisRaw(horizontalAxis),
            Input.GetAxisRaw(verticalAxis)
        ).normalized;
    }

    [OnUpdate]
    public void TheUpdate()
    {
        if (Input.GetKeyUp(InventoryKeyCode)) {
            Model.Set("IsInventoryEnabled", !Model.GetBool("IsInventoryEnabled"));
        }
    }
}