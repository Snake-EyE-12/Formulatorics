using System;
using System.Collections.Generic;
using System.Linq;
using Cobra.Utilities.Extensions;
using UnityEngine;

public static class ZoneHandler
{
    private static List<IZone> activeZones = new List<IZone>();

    public static void Add(IZone zone)
    {
        activeZones.Add(zone);
    }

    public static void Remove(IZone zone)
    {
        activeZones.Remove(zone);
    }

    public static List<IZone> GetSortedZonesByDistance(Vector2 center)
    {
        return activeZones.OrderBy((x) => x.GetDistance(center)).ToList();
    }
}

public class Zone : MonoBehaviour, IZone
{
    
    [SerializeField] private RectTransform slotContainer;
    [SerializeField] private RectTransform displayContainer;
    private void OnEnable()
    {
        ZoneHandler.Add(this);
    }

    private void OnDisable()
    {
        ZoneHandler.Remove(this);
    }

    public float GetDistance(Vector2 center)
    {
        return Vector2.SqrMagnitude(center - transform.position.XY());
    }

    [SerializeField] private bool open = true;
    public bool ValidFor()
    {
        return open; //space | type
    }
    
    private List<IZonable> zonables = new List<IZonable>();
    public void Join(IZonable zonable)
    {
        zonables.Add(zonable);
        zonable.GetZoneTransform().SetParent(slotContainer);
        zonable.GetDisplayTransform().SetParent(displayContainer);
        OrientSlots();
    }

    public void Leave(IZonable zonable)
    {
        zonables.Remove(zonable);
        OrientSlots();
    }

    private void OrientSlots()
    {
        int count = zonables.Count;
        if (count == 0) return;

        float halfWidth = slotContainer.rect.width * 0.5f;

        for (int i = 0; i < count; i++)
        {
            // Step 1: normalized position across range [0..1]
            float t = (count == 1) ? 0.5f : (float)i / (count - 1);

            // Step 2: map to local X
            float localX = Mathf.Lerp(-halfWidth, halfWidth, t);

            // Step 3: convert to world position along container’s local X axis
            Vector3 localPos = new Vector3(localX, 0, 0);
            zonables[i].GetZoneTransform().localPosition = localPos;

            // Step 4: maintain sibling order
            zonables[i].GetZoneTransform().SetSiblingIndex(i);
            
            // Step 5: maintain display sibling order
            zonables[i].GetDisplayTransform().SetSiblingIndex(i);
        }
    }


    public void Reorder(IZonable zonable, Vector2 anchorPoint)
    {
        Vector3 local = slotContainer.InverseTransformPoint(anchorPoint);

        float halfWidth = slotContainer.rect.width * 0.5f;
        float percent = Mathf.InverseLerp(-halfWidth, halfWidth, local.x);

        percent = Mathf.Clamp01(percent);

        int newIndex = Mathf.RoundToInt(percent * (zonables.Count - 1));

        zonables.Remove(zonable);
        zonables.Insert(newIndex, zonable);

        OrientSlots();
    }

}

public interface IZone
{
    public float GetDistance(Vector2 position);
    public bool ValidFor();
    public void Join(IZonable zonable);
    public void Leave(IZonable zonable);
    public void Reorder(IZonable zonable, Vector2 anchorPoint);
}