using System;
using System.Linq;
using UnityEngine;

public class CardZoneFinder : MonoBehaviour, ICardZoneFinder
{
    private IZone nearest;
    
    public bool Find(Vector2 origin, out IZone nearestZone)
    {
        nearestZone = null;
        IZone nearestLegalZone = ServiceLocator.Get<IZoneControl>().Zones.OrderBy((x) => x.GetDistance(origin)).FirstOrDefault(x => x.ValidFor());

        if (nearestLegalZone == null) return false;
        if (nearestLegalZone != nearest)
        {
            nearest = nearestLegalZone;
            nearestZone = nearestLegalZone;
            return true;
        }

        return false;
    }
}

public interface ICardZoneFinder
{
    public bool Find(Vector2 origin, out IZone nearestZone);
}