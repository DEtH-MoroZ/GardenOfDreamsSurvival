using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    public static bool Contains(this LayerMask mask, GameObject gameObject)
    {
        return (mask.value & (1 << gameObject.layer)) != 0;
    }

    public static LayerMask GetLayerMask(GameObject go)
    {
        LayerMask layerMask = 1 << go.layer;
        return layerMask;
    }
}