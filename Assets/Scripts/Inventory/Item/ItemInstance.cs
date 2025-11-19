using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance 
{
    public ItemScriptableObject data;

    public System.Guid UniqueID { get; private set; }

    public ItemInstance(ItemScriptableObject theData) {
        
        UniqueID = System.Guid.NewGuid();

        data = theData;

    }
}
