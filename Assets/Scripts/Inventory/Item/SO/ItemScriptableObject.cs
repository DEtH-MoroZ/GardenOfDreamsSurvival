using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptableObject : ScriptableObject
{
    public int id = 0;

    public bool stackable = false;
    public bool dropable = false;

    public Sprite sprite;

    public virtual void Use() // Player player?
    {
        // Base use logic, override for specific behaviors
        Debug.Log($"Using {this.name}");
    }

    public virtual void Drop() // Player player?
    {
        // Base use logic, override for specific behaviors
        Debug.Log($"Droping {this.name}");
    }

    public virtual void PickUp() // Player player?
    {
        // Base use logic, override for specific behaviors
        Debug.Log($"Picking up {this.name}");
    }
}