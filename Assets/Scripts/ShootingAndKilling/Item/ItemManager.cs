using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Item State Machine:
    [World] ←→ [Inventory] ←→ [Equipped]
        ↓           ↓           ↓
    World View  Inventory View Equipped View
*/

public class ItemManager : MonoBehaviour //class, that drives items around
{
    public ItemDatabase theItemDatabase;
    public Dictionary<int, ItemInstance> ItemDatabase = new Dictionary<int, ItemInstance>(); //guid and script, that holds item info representation

    public Transform itemWorldViewPrefab;


    private void Start()
    {
        TestSpawnHealingItem();
    }

    private void TestSpawnHealingItem()
    {
        //step one is to create ItemInstance
        //ItemInstance theHealingItem = new ItemInstance(
         //   )
        
        
        
        
        Transform healingSpawn = Instantiate(itemWorldViewPrefab);

    }
}
