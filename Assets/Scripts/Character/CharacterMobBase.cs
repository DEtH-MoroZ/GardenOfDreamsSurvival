using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMobBase : CharacterBase
{

    public override void Die()
    {
        Debug.Log("[Character Mob Base] Mob died.");
        Model.Dec("MobCountCurrent", 1);
        gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(false);
    }
}
