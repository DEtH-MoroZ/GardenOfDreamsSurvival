using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Utils;
using System.Collections;
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
    }

    [OnDelay(4f)]
    private void TestSpawnHealingItem()
    {        
        ItemInstance theHealingItem = new ItemInstance(theItemDatabase.GetItem(3));

        inGameItemDatabase.Add(theHealingItem.UniqueID, theHealingItem);

        Model.EventManager.Invoke("SpawnItem", inGameItemDatabase.GetValueByKeyOrNull(theHealingItem.UniqueID)); //additional make-sure that we use databased item
    }
}



