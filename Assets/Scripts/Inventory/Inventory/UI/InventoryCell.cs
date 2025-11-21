using TMPro;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviourExt
{
    [Header("Information Elements")]
    public RectTransform InfoPanel;
    public Image image;
    public TMP_Text text;
    public Button informationBackgrownButton;

    [Header("Interaction Elements")]
    public RectTransform InteractionPanel;
    public Button useButton;
    public Button dropButton;
    public Button interactionBackgrowndButton;

    InventorySlot _InventorySlot;

    [OnStart]
    public void TheStart()
    {
        informationBackgrownButton.onClick.AddListener(EnableInteractionPanel);
        interactionBackgrowndButton.onClick.AddListener(EnableInformationPanel);

        useButton.onClick.AddListener(Use);
        dropButton.onClick.AddListener(Drop);
    }

    public void SetinventorySlot (InventorySlot inventorySlot)
    {
        _InventorySlot = inventorySlot;

        image.sprite = _InventorySlot.GetFirstItemInstance().data.sprite;
        if (_InventorySlot.stackCount == 1)
        {
            text.text = _InventorySlot.GetFirstItemInstance().data.name;
        }
        else
        {
            text.text = _InventorySlot.GetFirstItemInstance().data.name + " " + _InventorySlot.stackCount;
        }
    }


    private void EnableInformationPanel () {
        InfoPanel.gameObject.SetActive(true);
        InteractionPanel.gameObject.SetActive(false);
    }

    private void EnableInteractionPanel () {
        InfoPanel.gameObject.SetActive(false);
        InteractionPanel.gameObject.SetActive(true);
    }

    private void Use () {
       
        Debug.Log($"[Inventory UI] Use item: GUID: {_InventorySlot.GetFirstItemInstance().UniqueID.ToString()}; Item Type: {_InventorySlot.GetFirstItemInstance().data.GetType().ToString()}; Item ID: {_InventorySlot.GetFirstItemInstance().data.id}");
    }

    private void Drop () {
        Debug.Log($"[Inventory UI] Drop item: GUID: {_InventorySlot.GetFirstItemInstance().UniqueID.ToString()}; Item Type: {_InventorySlot.GetFirstItemInstance().data.GetType().ToString()}; Item ID: {_InventorySlot.GetFirstItemInstance().data.id}");
        Model.Get<PlayerInventoryManager>("PlayerInventory").DropItem(_InventorySlot);
        Model.Set("InventoryDirty", true);

    }

    public void TheDestroy ()
    {
        Destroy(this);
    }
}
