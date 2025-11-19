using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingItemData", menuName = "Game/Healing Item Data")]
public class HealingItemScriptableObject : ItemScriptableObject
{
    public float HealAmmount = 25f;
}
