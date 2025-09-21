using System;
using System.Collections.Generic;
using System.Linq;
using Cobra.Utilities;
using UnityEngine;

public class Zone : MonoBehaviour, IZone
{
    [SerializeField] private RectTransform slotContainer;
    [SerializeField] private RectTransform displayContainer;
    [SerializeField] private RectTransform inputContainer;
    private void Start()
    {
        ServiceLocator.Get<IZoneControl>().Register(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Get<IZoneControl>().Deregister(this);
    }

    public float GetDistance(Vector2 point)
    {
        Vector3 localPoint = slotContainer.InverseTransformPoint(point);

        Rect rect = slotContainer.rect;
        float clampedX = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
        float clampedY = Mathf.Clamp(localPoint.y, rect.yMin, rect.yMax);

        Vector3 closestWorld = slotContainer.TransformPoint(new Vector3(clampedX, clampedY, 0));

        return ((Vector2)closestWorld - point).sqrMagnitude;
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
        zonable.OnZoneJoined(slotContainer, displayContainer, inputContainer);
        OrientSlots();
    }

    public void Leave(IZonable zonable)
    {
        zonables.Remove(zonable);
        OrientSlots();
    }

    [SerializeField] private Curve heightCurve;
    [SerializeField] private Curve rotationCurve;

    private void OrientSlots()
    {
        int count = zonables.Count;
        float divisor = Mathf.Max(count - 1, 1);
        if (count == 0) return;

        float halfWidth = (slotContainer.rect.width) * 0.5f;

        for (int i = 0; i < count; i++)
        {
            float t = (float)(i + 1) / (count + 1);
            float localX = Mathf.Lerp(-halfWidth, halfWidth, t);
            Vector3 localPos = new Vector3(localX, heightCurve.Evaluate(i / divisor), 0);
            float localRot =
                (count > 1)
                ? -rotationCurve.Evaluate(i / divisor)
                : 0;
            zonables[i].SlotTo(localPos, localRot, i);
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

    private void OnDrawGizmos()
    {
        if (slotContainer == null) return;

        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = slotContainer.localToWorldMatrix;
        Gizmos.DrawWireCube(slotContainer.rect.center, slotContainer.rect.size);
        Gizmos.matrix = oldMatrix;
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