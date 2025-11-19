using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviourExtBind
{
    public Transform InventoryCellPrefab;

    private bool isInventoryEnabled = false;

    private CharacterInventoryManager inventoryManager;

    [OnAwake]
    void TheAwake()
    {
        Model.Set("IsInventoryEnabled", isInventoryEnabled);
    }    

    [Bind("OnIsInventoryEnabledChanged")]
    private void ChangeInventoryState(bool value)
    {
        inventoryManager = Model.Get<PlayerInventoryManager>("PlayerInventory") as CharacterInventoryManager;

        Debug.Log($"[InventoryUI] Inventory enabled: {value}");
        this.gameObject.SetActive(value);
    }
    [OnEnable]
    void TheStart()
    {
        ChangeInventoryState(isInventoryEnabled);

        for (int a = 0; a < inventoryManager.inventorySlots.Count; a++)
        {
            Transform bumpInventoryCell = Instantiate(InventoryCellPrefab);
            bumpInventoryCell.SetParent(this.transform);
        }
    }
}
