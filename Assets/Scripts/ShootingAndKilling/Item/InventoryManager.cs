using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    void Awake()
    {
        // Load the item database
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        itemDatabase.Initialize();
    }

    public bool AddItem(int itemID, int quantity = 1)
    {
        ItemScriptableObject item = itemDatabase.GetItem(itemID);
        if (item == null) return false;

        // Try to stack if possible
        if (item.stackable == true)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.ItemID == itemID )
                {
                    slot.AddToStack(quantity);
                    return true;
                }
            }
        }

        // Add to new slot
        inventorySlots.Add(new InventorySlot(itemID, quantity));
        return true;
    }

    public void UseItem(int itemID)
    {
        ItemScriptableObject item = itemDatabase.GetItem(itemID);
        if (item != null)
        {
            Debug.Log("use item logic here");
            //item.Use(FindObjectOfType<Player>()); // Or pass player reference
            RemoveItem(itemID, 1);
        }
    }

    private void RemoveItem(int itemID, int quantity)
    {
        Debug.Log("use item logic here");
        // Implementation for removing items
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ItemID;
    public int Quantity;

    public InventorySlot(int itemID, int quantity)
    {
        ItemID = itemID;
        Quantity = quantity;
    }

    public void AddToStack(int amount) => Quantity += amount;
}