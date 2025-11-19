#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemDatabaseEditor : EditorWindow
{
    [MenuItem("Inventory/Update Item Database")]
    public static void UpdateItemDatabase()
    {
        string pathToDatabase = "Assets/ScriptableObjects/Inventory/Database/ItemDatabase.asset";
        // Find all InventoryItem assets
        string[] guids = AssetDatabase.FindAssets("t:ItemScriptableObject");
        ItemScriptableObject[] items = new ItemScriptableObject[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            items[i] = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
        }

        // Load or create the database
        ItemDatabase database = Resources.Load<ItemDatabase>("ItemDatabase");
        if (database == null)
        {
            database = ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(database, pathToDatabase);
        }

        database.allItems = items;
        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();

        Debug.Log($"[Item Database Update] Item database updated with {items.Length} items! Database located at: {pathToDatabase}");
    }
}
#endif