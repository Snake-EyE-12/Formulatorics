using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSlot : MonoBehaviour, ICardSlot
{
    private IZone currentZone;
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

    public Transform Transform() => transform;
}

public interface ICardSlot
{
    public void UpdateActiveZone(IZone zone, IZonable zonable);
    public void ReorderActiveZone(Vector2 anchor, IZonable zonable);
    
    public Transform Transform();
}