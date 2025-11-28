using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingItemData", menuName = "Game/Inventory/Healing Item Data")]
public class HealingItemScriptableObject : ItemScriptableObject
{
    public int HealAmmount = 25;

    public override void Use(CharacterInventoryManager CIM, out bool RemoveAfterUse) // reference to the one, who use it
    {
        CIM.characterBase.Heal(HealAmmount);
        Debug.Log($"Healing item is used by {CIM.gameObject.name}");
        RemoveAfterUse = removeAfterUse;
    }
}
