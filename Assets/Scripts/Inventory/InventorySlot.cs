using System.Collections.Generic;

[System.Serializable]
public class InventorySlot
{
    private List<ItemInstance> _itemInstances;

    public InventorySlot(ItemInstance itemInstance)
    {
        _itemInstances = new List<ItemInstance>();
        _itemInstances.Add(itemInstance);
    }

    public ItemInstance GetFirstItemInstance()
    {
        return _itemInstances[0];
    }

    public void RemoveFirstItemInstance()
    {
        _itemInstances.RemoveAt(0);
    }
    public void AddToStack(ItemInstance itemInstance)
    {
        _itemInstances.Add(itemInstance);
    }
    public int stackCount
    {
        get { return _itemInstances.Count; }
    }

    public int itemID
    {
        get { return _itemInstances[0].data.id; }
    }

    public bool isStackable
    {
        get { return _itemInstances[0].data.stackable; }
    }

    public override string ToString()
    {
        return $"[ItemInstance] Name: {_itemInstances[0].data.name}; Item Qantity {_itemInstances.Count};";
    }
    
}