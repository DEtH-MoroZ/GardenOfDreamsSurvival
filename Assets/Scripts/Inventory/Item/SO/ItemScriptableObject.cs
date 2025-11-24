using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptableObject : ScriptableObject // Base use logic, override for specific behaviors
{
    public int id = 0;

    public bool stackable = false;
    public bool dropable = false;
    public bool removeAfterUse = false;
    public bool equippable = false;

    public Sprite sprite;

    public virtual void Use(CharacterInventoryManager CIM, out bool removeAfterUse) // reference to the one, who use it
    {
        
        Debug.Log($"Using {this.name} by {CIM.gameObject.name}");
        removeAfterUse = false;
    }

    public virtual void Drop(CharacterInventoryManager CIM)
    {
        
        Debug.Log($"Droping {this.name} by {CIM.gameObject.name}");
    }

    public virtual void PickUp(CharacterInventoryManager CIM)
    {
        
        Debug.Log($"Picking up {this.name} by {CIM.gameObject.name}");
    }
}