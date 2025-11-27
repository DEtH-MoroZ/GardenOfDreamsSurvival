using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInventoryManager : CharacterInventoryManager
{
    [OnDelay(0.1f)]
    void RequestStartLoot()
    {
        Model.EventManager.Invoke("ItemManager_RequestItemSpawnForMobs", this);
    }

    public void OnDeath()
    {
        DropAllItems();
    }

    public void DropAllItems()
    {
        int iterator = 0;
        for (int a = 0; a < inventorySlots.Count; a++)
        {
            int count = inventorySlots[a].stackCount;
            for (int b = 0; b < count; b++)
            {
                iterator++;
                DropItem(inventorySlots[a]);
            }
        }
    }
}
