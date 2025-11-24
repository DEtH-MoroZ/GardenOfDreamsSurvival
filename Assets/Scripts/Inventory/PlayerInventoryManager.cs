using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [SerializeField] private LayerMask itemLayer;

    [SerializeField] private float ProximityRadius = 0.5f;

    [OnStart]
    void TheStart()
    {
        Model.Set("PlayerInventory", this);
    }

    [OnRefresh(0.1f)]
    private void CheckProximity()
    {
        List<GameObject> proximityResult = Model.Get<GameObjectGrid>("GameObjectGrid").CheckProximityByLayer(ProximityRadius, transform.position.x, transform.position.y, itemLayer);

        for (int a = 0; a < proximityResult.Count; a++)
        {
            ItemWorldRepresentation foundItems = proximityResult[a].GetComponent<ItemWorldRepresentation>();
            for (int b = 0; b < foundItems.GetItemInstances().Count; b++)
            {
                if (AddItem(foundItems.GetItemInstances()[b]))
                {
                    Model.Set("InventoryDirty", true); //needed to mark inventory ui for update                    
                }
            }
            foundItems.PickUp();
        }
    }
}
