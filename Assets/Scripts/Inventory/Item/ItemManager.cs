using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Utils;
using System.Collections.Generic;
using UnityEngine;

/*
Item State Machine:
    [World] ←→ [Inventory] ←→ [Equipped]
        ↓           ↓           ↓
    World View  Inventory View Equipped View
*/

public class ItemManager : MonoBehaviourExt //class, that drives items around
{
    public ItemDatabase theItemDatabase; //holds scriptable objects, each is uniqe
    public Dictionary<System.Guid, ItemInstance> inGameItemDatabase = new Dictionary<System.Guid, ItemInstance>(); //guid and class instance, that holds item info representation

    [OnStart]
    private void TheStart()
    {
        theItemDatabase.Initialize();
        Model.EventManager.AddAction<System.Guid>(nameof(ItemManager) + "_" + nameof(DestroyItem), DestroyItem);
        Model.EventManager.AddAction<MobInventoryManager>(nameof(ItemManager) + "_" + nameof(RequestItemSpawnForMobs), RequestItemSpawnForMobs);
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<System.Guid>(nameof(ItemManager) + "_" + nameof(DestroyItem), DestroyItem);
        Model.EventManager.RemoveAction<MobInventoryManager>(nameof(ItemManager) + "_" + nameof(RequestItemSpawnForMobs), RequestItemSpawnForMobs);
    }

    public void DestroyItem(System.Guid id)
    {
        if (inGameItemDatabase.ContainsKey(id))
        {
            inGameItemDatabase.Remove(id);
        }
        else
        {
            Debug.LogWarning($"[ItemManager] Trying to destroy non-existent item, GUID = {id}");
        }
    }

    [OnDelay(4f)]
    private void TheTest()
    {
        for (int a = 0; a < 6; a++)
        {
            TestSpawnHealingItem(Random.Range(-20f, 20f), Random.Range(-20f, 20f));
        }

        Debug.Log($"Total spawned {inGameItemDatabase.Count}");
    }

    private void TestSpawnHealingItem(float x, float y)
    {
        ItemInstance theHealingItem = new ItemInstance(theItemDatabase.GetItem(3));

        inGameItemDatabase.Add(theHealingItem.UniqueID, theHealingItem);

        Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theHealingItem.UniqueID), x, y); //additional make-sure that we use databased item
    }

    [OnDelay(5f)]
    private void TestSpawnWeapons()
    {
        ItemInstance theAk = new ItemInstance(theItemDatabase.GetItem(2));

        inGameItemDatabase.Add(theAk.UniqueID, theAk);

        Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theAk.UniqueID), 20f, 20f);

        for (int a = 0; a < 30; a++)
        {
            ItemInstance theAmmoForAk = new ItemInstance(theItemDatabase.GetItem(0));

            inGameItemDatabase.Add(theAmmoForAk.UniqueID, theAmmoForAk);

            Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theAmmoForAk.UniqueID), 21f, 21f);
        }

        ItemInstance theMakarov = new ItemInstance(theItemDatabase.GetItem(6));

        inGameItemDatabase.Add(theMakarov.UniqueID, theMakarov);

        Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theMakarov.UniqueID), -20f, -20f);

        for (int a = 0; a < 30; a++)
        {
            ItemInstance theAmmoForMakarov = new ItemInstance(theItemDatabase.GetItem(1));

            inGameItemDatabase.Add(theAmmoForMakarov.UniqueID, theAmmoForMakarov);

            Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theAmmoForMakarov.UniqueID), -21f, -21f);
        }

        ItemInstance theMachete = new ItemInstance(theItemDatabase.GetItem(5));

        inGameItemDatabase.Add(theMachete.UniqueID, theMachete);

        Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theMachete.UniqueID), -20f, 0f);
    }


    private int itemRequestIteration = 0; // 0 healthpack, 1 ammo for makarov, 2 ammo for ak
    private void RequestItemSpawnForMobs(MobInventoryManager CIM)
    {
        switch (itemRequestIteration%3)
        {
            case 0: SpawnItemIntoCharacter(CIM, 1, 3); break;
            case 1: SpawnItemIntoCharacter(CIM, 15, 1); break;
            case 2: SpawnItemIntoCharacter(CIM, 30, 0); break;
            default: break;
        }
        itemRequestIteration++;
    }
    
    private void SpawnItemIntoCharacter(CharacterInventoryManager CIM, int itemCount, int id)
    {
        for (int a = 0; a < itemCount; a++)
        {
            ItemInstance theItem = new ItemInstance(theItemDatabase.GetItem(id));
            inGameItemDatabase.Add(theItem.UniqueID, theItem);
            CIM.AddItem(theItem);
        }
    }

}



