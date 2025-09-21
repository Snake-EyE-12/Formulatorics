using Cobra.DesignPattern;
using UnityEngine;

public class DragHandler : MonoBehaviour, IDragControl
{
    private void Awake()
    {
        ServiceLocator.Register<IDragControl>(this);
    }
    
    private IDraggable previous;
    public void StartDrag(IDraggable candidate, Vector2 location)
    {
        previous = candidate;
        candidate.OnDragBegin(location);
    }

    public void EndDrag(IDraggable candidate, Vector2 location)
    {
        candidate.OnDragEnd(location);
    }

    public void Drag(IDraggable candidate, Vector2 location)
    {
        candidate.OnDrag(location);
    }
}

public interface IDragControl : IService
{
    public void StartDrag(IDraggable candidate, Vector2 location);
    public void EndDrag(IDraggable candidate, Vector2 location);
    public void Drag(IDraggable candidate, Vector2 location);
}

