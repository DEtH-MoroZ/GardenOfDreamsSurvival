using System.Collections.Generic;
using UnityEngine;

//stores all item scrriptable objects all across project

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemScriptableObject[] allItems;

    private Dictionary<int, ItemScriptableObject> itemDictionary;

    public void Initialize()
    {
        itemDictionary = new Dictionary<int, ItemScriptableObject>();

        foreach (var item in allItems)
        {
            if (!itemDictionary.ContainsKey(item.id))
            {
                itemDictionary.Add(item.id, item);
            }
            else
            {
             Debug.LogWarning($"[ItemDatabase] Duplicate item ID found: {item.id}");              
            }
        }
    }

    public ItemScriptableObject GetItem(int id)
    {
        if (itemDictionary.ContainsKey(id))
            return itemDictionary[id];

        Debug.LogWarning($"[ItemDatabase]Item with ID {id} not found!");
        return null;
    }

    public bool ItemExists(int id)
    {
        return itemDictionary.ContainsKey(id);
    }
}