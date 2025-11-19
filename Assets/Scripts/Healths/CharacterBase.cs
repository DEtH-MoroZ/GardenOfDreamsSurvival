using AxGrid;
using AxGrid.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterBase : MonoBehaviourExt, IDamageable
{
    [SerializeField] protected CharacterData characterData;
    [SerializeField] protected int currentHealth;

    public int MaxHeaths => characterData.MaxHealth;

    public int Health => currentHealth;
    public bool IsAlive => currentHealth > 0;
    public bool IsValidTarget => IsAlive;

    public float atackRange = 10f;

    public CharacterBase TargetForAtack;
    public LayerMask enemyLayermask;

    CharacterHealthsBarUI healthsBarUI;

    [OnAwake]
    void TheAwake()
    {
        currentHealth = characterData.MaxHealth;
    }

    [OnStart]
    void TheStart()
    {
        RegisterAtGamobjectGrid();
        healthsBarUI = GetComponent<CharacterHealthsBarUI>();
    }

    public void TakeDamage(int damage)
    {
        if(!IsAlive) return;
        currentHealth -= damage;
        if (currentHealth <= 0)        {
            Die();
        }        
        healthsBarUI?.OnHealthsChanged(currentHealth, MaxHeaths);
    }

    [OnRefresh(1f)]
    public void FindTargetAndAtack()
    {
        GameObjectGrid _Grid = Model.Get<GameObjectGrid>("GameObjectGrid");
        GameObject tar = _Grid?.FindClosestByRadiusAndLayer(transform.position, atackRange, enemyLayermask);
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
    public void Atack ()
    {
        if (canAtack == false) { return; }
        if (TargetForAtack != null)
        {
            //Debug.Log(TargetForAtack.name);

            if (TargetForAtack.IsAlive == false) { return; }
            TargetForAtack.TakeDamage(characterData.Damage);
            Debug.Log(Vector3.Distance(TargetForAtack.transform.position, transform.position) + " " + atackRange);
        }
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, MaxHeaths);
    }

    public virtual void Die()
    {
        //death logic, despawn should be after timer.
        //OnDespawned?.Invoke(this);
        Debug.Log("die");
        Model.Dec("MobCountCurrent", 1);
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
        Handles.DrawWireDisc(transform.position, Vector3.back, atackRange);
    }
#endif
}
