using System.Collections.Generic;

[System.Serializable]
public class InventorySlot
{
    private List<ItemInstance> _itemInstances;

    private bool _isEqupped = false;

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

    public void Equip ()
    {
        _isEqupped = true;
    }

    public void UnEquip ()
    {
        _isEqupped = false;
    }

    public bool isEqupped()
    {
        return _isEqupped;
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

    public string GetItemType()
    {
        return _itemInstances[0].data.GetType().ToString();
    }
    
}