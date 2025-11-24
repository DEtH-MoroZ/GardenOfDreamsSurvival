using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class CharacterInventoryManager : MonoBehaviourExt
{    

    [HideInInspector]
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    [HideInInspector]
    public CharacterBase characterBase;

    [OnAwake]
    void TheAwake()
    {
        characterBase = GetComponent<CharacterBase>();
    }

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
        inventorySlot.GetFirstItemInstance().data.Drop(this);
        inventorySlot.RemoveFirstItemInstance();
        
        if (inventorySlot.stackCount == 0)
        {
            inventorySlots.Remove(inventorySlot);
        }
        Model.Set("InventoryDirty", true);
    }

    public void UseItem(InventorySlot inventorySlot)
    {
        inventorySlot.GetFirstItemInstance().data.Use(this, out bool removeAfterUse);
        if (removeAfterUse == true)
        {
            Model.EventManager.Invoke(nameof(ItemManager) + "_" + nameof(ItemManager.DestroyItem), inventorySlot.GetFirstItemInstance().UniqueID);
            inventorySlot.RemoveFirstItemInstance();
            if (inventorySlot.stackCount == 0)
            {
                inventorySlots.Remove(inventorySlot);
            }
        }
        Model.Set("InventoryDirty", true);
    }  
}