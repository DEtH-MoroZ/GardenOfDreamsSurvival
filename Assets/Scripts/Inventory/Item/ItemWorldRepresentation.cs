using UnityEngine;
using AxGrid.Base;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemWorldRepresentation : MonoBehaviourExt
{
    List<ItemInstance> _itemInstances;
    private SpriteRenderer _spriteRenderer;

    public int StackCount = 0;

    public void AddItemInstance(ItemInstance itemInstance)
    {
        if (_itemInstances == null)
        {
            _itemInstances = new List<ItemInstance>();
            _itemInstances.Add(itemInstance);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _itemInstances.Last().data.sprite;
            Debug.Log($"[ItemInstance] Item UniqueID = {_itemInstances.Last().UniqueID}, Type = {_itemInstances.Last().data.GetType().ToString()}, Item Database ID = {_itemInstances.Last().data.id} Spawned!");
        }
        else
        {
            _itemInstances.Add(itemInstance);
            Debug.Log($"[ItemInstance] Item UniqueID = {_itemInstances.Last().UniqueID}, Type = {_itemInstances.Last().data.GetType().ToString()}, Item Database ID = {_itemInstances.Last().data.id} Spawned!");
        }
        StackCount = _itemInstances.Count;
    }

    public List<ItemInstance> GetItemInstances() { return _itemInstances; }

    public void PickUp ()
    {
        Model.EventManager.Invoke("Despawn", gameObject);
        Debug.Log($"[ItemInstance] Type = {_itemInstances.Last().data.GetType().ToString()}, Item Database ID = {_itemInstances.Last().data.id}, item count = {_itemInstances.Count} Picked up!");
    }
}
