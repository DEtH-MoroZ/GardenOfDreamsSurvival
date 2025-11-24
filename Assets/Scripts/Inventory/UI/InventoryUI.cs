using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviourExtBind
{
    public Transform InventoryCellPrefab;
    public Transform CellContainer;

    private bool isInventoryEnabled = false;

    [OnStart]
    void TheStart()
    {
        Model.Set("IsInventoryEnabled", isInventoryEnabled);
        Model.Set("InventoryDirty", false);
    }

    [Bind("OnIsInventoryEnabledChanged")]
    private void ChangeInventoryState(bool value)
    {        
        
        
        Debug.Log($"[InventoryUI] Inventory enabled: {value}");
        CellContainer.gameObject.SetActive(value);

        if (value == true)
        {
            PopulateInventory();
        }
        else
        {
            ClearInventory();
        }
    }
    
    void PopulateInventory()
    {   
        if (Model.Get<PlayerInventoryManager>("PlayerInventory") != null)
        {
            for (int a = 0; a < (Model.Get<PlayerInventoryManager>("PlayerInventory")).inventorySlots.Count; a++)
            {
                Transform bumpInventoryCell = Instantiate(InventoryCellPrefab);
                bumpInventoryCell.SetParent(CellContainer);
                bumpInventoryCell.GetComponent<InventoryCell>().SetinventorySlot(
                    (Model.Get<PlayerInventoryManager>("PlayerInventory"))
                    .inventorySlots[a]);
            }
        }
        else
        {
            Debug.Log("[Inventory UI] No Player Inventory Presented");
        }
    }

    
    void ClearInventory ()
    {
        foreach (Transform child in CellContainer)
        {
            Destroy(child.gameObject);
        }
    }

    [Bind("OnInventoryDirtyChanged")]
    void UpdateInventory(bool isDirty)
    {
        if (Model.GetBool("IsInventoryEnabled", false) == false) return;
        if (isDirty == false) return;
        ClearInventory();
        PopulateInventory();
        Model.Set("InventoryDirty", false);
    }
}
