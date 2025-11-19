using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviourExt
{
    public LayerMask itemLayer;

    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private float ProximityRadius = 0.5f;

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
        //ItemScriptableObject item = itemDatabase.GetItem(itemID);
        //if (item == null) return false;
        /*
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
        */
        // Add to new slot
        inventorySlots.Add(new InventorySlot(itemInstance));
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

    private void RemoveItem(int itemID, int quantity = 1)
    {
        Debug.Log("use item logic here");
        // Implementation for removing items
    }

    [OnRefresh(0.1f)]
    private void CheckProximity()
    {
        List<GameObject> proximityResult = Model.Get<GameObjectGrid>("GameObjectGrid").CheckProximityByLayer(ProximityRadius, transform.position.x, transform.position.y, itemLayer);

        for (int a = 0; a < proximityResult.Count; a++)
        {
            ItemWorldRepresentation foundItem = proximityResult[a].GetComponent<ItemWorldRepresentation>();
            AddItem(foundItem.GetItemInstance());
            proximityResult[a].BroadcastMessage("PickUp", SendMessageOptions.DontRequireReceiver);
            
        }
    }
} 

[System.Serializable]
public class InventorySlot
{   
    public ItemInstance _itemInstance;

    public InventorySlot(ItemInstance itemInstance)
    {
        _itemInstance = itemInstance;
    }

    //public void AddToStack(int amount) => Quantity += amount;
}