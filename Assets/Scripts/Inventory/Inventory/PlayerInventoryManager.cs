using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [OnStart]
    void TheStart()
    {
        Model.Set("PlayerInventory", this);
    }
}
