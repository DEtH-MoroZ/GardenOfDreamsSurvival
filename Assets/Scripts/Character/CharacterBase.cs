using AxGrid;
using AxGrid.Base;
using ExpressoBits.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterBase : MonoBehaviourExt
{
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int atackRange;

    public int MaxHeaths => characterData.MaxHealth;
    public int Health => currentHealth;
    public bool IsAlive => currentHealth > 0;
    public bool IsValidTarget => IsAlive;

    public float AtackRange => characterData.AttackRange;

    public CharacterBase TargetForAtack;
    public LayerMask enemyLayermask;

    CharacterHealthsBarUI healthsBarUI;
    CharacterDamageIndication damageIndication;

    CharacterInventoryManager inventoryManager;

    [OnStart]
    void TheStart()
    {
        currentHealth = characterData.MaxHealth;
        RegisterAtGamobjectGrid();
        healthsBarUI = GetComponent<CharacterHealthsBarUI>();
        damageIndication = GetComponent<CharacterDamageIndication>();
        inventoryManager = GetComponent<CharacterInventoryManager>();
    }

    public void TakeDamage(int damage)
    {
        if(!IsAlive) return;
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }        
        healthsBarUI?.OnHealthChanged(currentHealth, MaxHeaths);
        damageIndication?.OnHealthChanged();
    }

    [OnRefresh(1f)]
    public virtual void FindTargetAndAtack()
    {
        GameObjectGrid _Grid = Model.Get<GameObjectGrid>("GameObjectGrid");
        GameObject tar;
        if (inventoryManager.GetEquippedItem() == null)
        {
            tar = _Grid?.FindClosestByRadiusAndLayer(transform.position, AtackRange, enemyLayermask);
        }
        else
        {
            tar = _Grid?.FindClosestByRadiusAndLayer(transform.position, (inventoryManager.GetEquippedItem().data as WeaponScriptableObject).Range, enemyLayermask);
        }

        if (tar != null)
        {
            TargetForAtack = tar.GetComponent<CharacterBase>();
        }
        else
        {
            TargetForAtack = null;
        }
    }

    public bool canAtack = false;
    [OnRefresh(1f)]
    public virtual void Atack ()
    {
        if (canAtack == false) { return; }
        
        if (TargetForAtack != null)
        {
            

            if (TargetForAtack.IsAlive == false) { TargetForAtack = null; return; }
            
            if (inventoryManager.GetEquippedItem() == null) { //bare hands, base damage
                TargetForAtack.TakeDamage(characterData.Damage);
                Debug.Log(Vector3.Distance(TargetForAtack.transform.position, transform.position) + " " + atackRange);
            }
            else
            { //something equpped

                if (inventoryManager.GetEquippedItem().data.Interact(inventoryManager, out bool removeAfterInteract))
                {
                    TargetForAtack.TakeDamage((inventoryManager.GetEquippedItem().data as WeaponScriptableObject).Damage);
                    Debug.Log(Vector3.Distance(TargetForAtack.transform.position, transform.position) + " " + atackRange);
                }
                
            }
                
        }
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, MaxHeaths);
        healthsBarUI?.OnHealthChanged(currentHealth, MaxHeaths);
    }


    public virtual void Die()
    {
        //death logic, despawn should be after timer.
        //OnDespawned?.Invoke(this);
        
        gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(false);
    }

    private Vector3 prevPosition;
    private void RegisterAtGamobjectGrid ()
    {
        Model.Get<GameObjectGrid>("GameObjectGrid").Add(this.gameObject);
        prevPosition = transform.position;
    }
    [OnRefresh(0.2f)]
    private void TrackAtGamobjectGrid ()
    {
        Model.Get<GameObjectGrid>("GameObjectGrid").Track(this.gameObject, prevPosition);
        prevPosition = gameObject.transform.position;
    }



#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        // Draw a wire disc facing forward (Vector3.back) in 2D space
        if (inventoryManager.GetEquippedItem() == null)
        {
            Handles.DrawWireDisc(transform.position, Vector3.back, AtackRange);
        }
        else
        {
            Handles.DrawWireDisc(transform.position, Vector3.back, (inventoryManager.GetEquippedItem().data as WeaponScriptableObject).Range);
        }
    }
#endif
}
