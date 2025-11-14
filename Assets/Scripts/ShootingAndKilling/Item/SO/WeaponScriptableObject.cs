using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Item Data")]
public class WeaponScriptableObject : ItemScriptableObject
{
    public bool needAmmo = true;
    public float Range = 1f;
    public float RoundsPerSecond = 1f;
    public float Damage = 25f;

    public bool isAutomaic = false;
    public bool canMoveWhileAtack = false;

    public AmmoScriptableObject ammo;
}
