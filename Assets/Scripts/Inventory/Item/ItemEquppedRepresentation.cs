using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemEquppedRepresentation : MonoBehaviour
{
    private ItemInstance _itemInstance;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _itemInstance = null;
        spriteRenderer.sprite = null;
    }

    public void SetEmpty()
    {
        _itemInstance = null;
        spriteRenderer.sprite = null;
    }

    public void SetItem(ItemInstance itemInstance)
    {
        _itemInstance = itemInstance;
        spriteRenderer.sprite = _itemInstance.data.sprite;
    }

}
