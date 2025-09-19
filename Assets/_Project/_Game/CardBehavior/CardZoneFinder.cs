using System;
using System.Linq;
using UnityEngine;

public class CardZoneFinder : MonoBehaviour, ICardZoneFinder
{
    private IZone nearest;
    public Action<IZone> OnNearestZoneChange { get; set; }
    
    
    public void Search(Vector2 origin)
    {
        IZone nearestLegalZone = ZoneHandler.GetSortedZonesByDistance(origin).FirstOrDefault(x => x.ValidFor());
        if (nearestLegalZone == null) return;
        if (nearestLegalZone != nearest) ChangeNearestZone(nearestLegalZone);
    }

    private void ChangeNearestZone(IZone zone)
    {
        nearest = zone;
        OnNearestZoneChange(nearest);
    }
}

public interface ICardZoneFinder
{
    public Action<IZone> OnNearestZoneChange {get; set;}
    public void Search(Vector2 origin);
}