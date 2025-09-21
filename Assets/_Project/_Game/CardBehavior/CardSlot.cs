using System.Collections.Generic;
using System.Linq;
using Cobra.Utilities.Extensions;
using UnityEngine;

public class CardSlot : MonoBehaviour, IZoneSlot
{
    private IZone currentZone;
    private float slotAngle;
    public void UpdateActiveZone(IZone zone, IZonable zonable)
    {
        currentZone?.Leave(zonable);
        currentZone = zone;
        currentZone.Join(zonable);
    }

    public void ReorderActiveZone(Vector2 anchor, IZonable zonable)
    {
        if(currentZone == null) return;
        currentZone.Reorder(zonable, anchor);
    }

    public void SlotTo(Vector2 position, float angle)
    {
        transform.localPosition = position;
        slotAngle = angle;
    }

    public void SetOffset(Vector2 offset)
    {
        followOffset = offset;
    }

    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    public void SetSiblingOrder(int siblingIndex)
    {
        transform.SetSiblingIndex(siblingIndex);
    }

    private Vector2 followOffset;
    public Vector2 Origin()
    {
        return transform.position.xy() + followOffset;
    }

    public float Angle() => slotAngle;
}

public interface IZoneSlot : ILayerOrderable, IPositionFollowable, IRotationFollowable
{
    public void UpdateActiveZone(IZone zone, IZonable zonable);
    public void ReorderActiveZone(Vector2 anchor, IZonable zonable);
    public void SlotTo(Vector2 position, float angle);
    public void SetOffset(Vector2 offset);

}