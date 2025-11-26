using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [SerializeField] private LayerMask itemLayer;

    [SerializeField] private float ProximityRadius = 0.5f;

    private bool itemAdded = false;

    [OnStart]
    void TheStart()
    {
        Model.Set("PlayerInventory", this);
    }

    [OnRefresh(0.1f)]
    private void CheckProximity()
    {
        List<GameObject> proximityResult = Model.Get<GameObjectGrid>("GameObjectGrid").CheckProximityByLayer(ProximityRadius, transform.position.x, transform.position.y, itemLayer);

        itemAdded = false;

        for (int a = 0; a < proximityResult.Count; a++)
        {
            ItemWorldRepresentation foundItems = proximityResult[a].GetComponent<ItemWorldRepresentation>();
            for (int b = 0; b < foundItems.GetItemInstances().Count; b++)
            {
                if (AddItem(foundItems.GetItemInstances()[b]))
                {
                     itemAdded = true;
                }
            }
            foundItems.PickUp();
        }
        if (itemAdded == true)
        {
            Model.Set("InventoryDirty", true);
        }
    }

    public override void UseItem(InventorySlot inventorySlot)
    {
        base.UseItem(inventorySlot);

        Model.Set("InventoryDirty", true);
    }

    public override void DropItem(InventorySlot inventorySlot)
    {
        base.DropItem(inventorySlot);

        Model.Set("InventoryDirty", true);
    }

    public override bool InteractWithItem(InventorySlot inventorySlot)
    {
        bool boolToReturn = base.InteractWithItem(inventorySlot);
        if (boolToReturn == true)
        {
            Model.Set("InventoryDirty", true);
        }
        return boolToReturn;
    }
}
