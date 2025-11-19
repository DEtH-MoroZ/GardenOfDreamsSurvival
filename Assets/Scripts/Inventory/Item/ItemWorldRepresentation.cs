using AxGrid.Base;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemWorldRepresentation : MonoBehaviourExt
{
    ItemInstance _itemInstance;
    private SpriteRenderer _spriteRenderer;

    public void SetItemInstance (ItemInstance itemInstance)
    {
        _itemInstance = itemInstance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _itemInstance.data.sprite;
        Debug.Log($"[ItemInstance] Item UniqueID = {_itemInstance.UniqueID}, Type = {_itemInstance.GetType().ToString()}, Item Database ID = {_itemInstance.data.id} Spawned!");
    }

    public ItemInstance GetItemInstance() { return _itemInstance; }

    public void PickUp ()
    {
        Model.EventManager.Invoke("Despawn", gameObject);
        Debug.Log($"[ItemInstance] Item UniqueID = {_itemInstance.UniqueID}, Type = {_itemInstance.data.GetType().ToString()}, Item Database ID = {_itemInstance.data.id} Picked up!");
    }
}
