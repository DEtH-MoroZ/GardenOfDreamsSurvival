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

    private ItemInstance equppedItem;

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
    public virtual void DropItem(InventorySlot inventorySlot)
    {        
        if (inventorySlot.GetFirstItemInstance().UniqueID == equppedItem?.UniqueID)
        {
            equppedItem = null;
            inventorySlot.UnEquip();
            equppedItem = null;
        }

        Model.EventManager.Invoke("SpawnItem", inventorySlot.GetFirstItemInstance(), transform.position.x+1f, transform.position.y+1f);
                
        inventorySlot.GetFirstItemInstance().data.Drop(this);
        inventorySlot.RemoveFirstItemInstance();
        
        if (inventorySlot.stackCount == 0)
        {
            inventorySlots.Remove(inventorySlot);
        }        
    }

    public virtual void UseItem(InventorySlot inventorySlot)
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
            
            return;
        }
        if (inventorySlot.GetFirstItemInstance().data.equippable == true) {

            for (int a = 0; a < inventorySlots.Count; a++)
            {
                inventorySlots[a].UnEquip();
            }
            equppedItem = inventorySlot.GetFirstItemInstance();
            inventorySlot.Equip();
            Debug.Log($"[InventoryManager] Item equpped: id = {inventorySlot.itemID}; type = {inventorySlot.GetItemType()}");

            return;
        }        
    }

    public virtual bool InteractWithItem(InventorySlot inventorySlot)
    {
        inventorySlot.GetFirstItemInstance().data.Interact(this, out bool removeAfterInteract);
        if (removeAfterInteract)
        {
            inventorySlot.RemoveFirstItemInstance();
            if (inventorySlot.stackCount == 0)
            {
                inventorySlots.Remove(inventorySlot);
            }
            return true;
        }
        else return false;
    }

    public InventorySlot FindItemByType (ItemScriptableObject itemSO)
    {
        //Debug.Log( itemSO.GetType().ToString());
        for (int a = 0; a < inventorySlots.Count; a++)
        {
            if (inventorySlots[a].GetFirstItemInstance().data == itemSO)
            {
                //Debug.Log($"found in {inventorySlots[a].GetFirstItemInstance().data.GetType()}");
                return inventorySlots[a];
            }
        }

        return null;
    }

    public ItemInstance GetEquippedItem ()
    {
        return equppedItem;
    }
}