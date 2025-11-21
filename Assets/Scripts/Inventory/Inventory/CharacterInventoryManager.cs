using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviourExt
{    
    [HideInInspector]
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
/*
    void Awake()
    {
        // Load the item database
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        itemDatabase.Initialize();
    }
*/
    public bool AddItem(ItemInstance itemInstance)
    {
        if (itemInstance.data.stackable == true)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.itemID == itemInstance.data.id)
                {
                    slot.AddToStack(itemInstance);
                    return true;
                }
            }
        }

        // Add to new slot
        inventorySlots.Add(new InventorySlot(itemInstance));

        return true; //for future situation, where inventory is full
    }
    public void DropItem(InventorySlot inventorySlot)
    {        
        Model.EventManager.Invoke("SpawnItem", inventorySlot.GetFirstItemInstance(), transform.position.x+1f, transform.position.y+1f);
        inventorySlot.RemoveFirstItemInstance();
        
        if (inventorySlot.stackCount == 0)
        {
            inventorySlots.Remove(inventorySlot);
        }
    }

    public void UseItem(ItemInstance itemInstance)
    {


        /*
        ItemScriptableObject item = itemDatabase.GetItem(itemID);
        if (item != null)
        {
            Debug.Log("use item logic here");
            //item.Use(FindObjectOfType<Player>()); // Or pass player reference
            RemoveItem(itemID, 1);
        }*/
    }

    private void RemoveItem(ItemInstance itemInstance, int quantity = 1)
    {
        Debug.Log("use item logic here");
        // Implementation for removing items
    }
    
}