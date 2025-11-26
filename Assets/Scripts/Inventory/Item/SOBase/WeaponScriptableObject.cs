using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Item Data")]
public class WeaponScriptableObject : ItemScriptableObject
{
    public bool needAmmo = true;
    public float Range = 1f;
    public float RoundsPerMinute = 60;
    public int Damage = 25;

    public bool isAutomaic = false;
    public bool canMoveWhileAtack = false;

    public AmmoScriptableObject ammo;

    public override bool Interact(CharacterInventoryManager CIM, out bool removeAfterInteraction)
    {
        removeAfterInteraction = base.removeAfterInteract;
        return Shoot(CIM);
    }

    private bool Shoot(CharacterInventoryManager CIM)
    {
        if (ammo == null) //ammo not requred
        {
            Debug.Log("shot");
            return true;
        }

        else //ammo requred
        {
            InventorySlot ammoInvSlot = CIM.FindItemByType(ammo);
            if (ammoInvSlot == null) { //ammo not found in inventory
                return false;
            }
            CIM.InteractWithItem(ammoInvSlot);
            return true;
        }
        
    }
}
