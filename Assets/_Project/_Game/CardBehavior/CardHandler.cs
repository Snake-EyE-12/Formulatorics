using System;
using System.Collections.Generic;
using System.Linq;
using Cobra.Utilities.Extensions;
using UnityEngine;

public class CardHandler : MonoBehaviour, IZonable
{
    private ICardSlot slotHandler;
    private ICardAnchor anchorHandler;
    private ICardDisplay displayHandler;
    private ICardInputReader inputHandler;
    private ICardZoneFinder zoneHandler;


    private IAnchorFollowFinder draggingAnchorState;
    private IAnchorFollowFinder slottedAnchorState;
    private IAnchorFollowFinder activeAnchorState;
    private void Awake()
    {
        slotHandler = GetComponentInChildren<ICardSlot>();
        anchorHandler = GetComponentInChildren<ICardAnchor>();
        displayHandler = GetComponentInChildren<ICardDisplay>();
        inputHandler = GetComponentInChildren<ICardInputReader>();
        zoneHandler = GetComponent<ICardZoneFinder>();
        
        draggingAnchorState = new DraggingAnchorPositionFinder();
        slottedAnchorState = new SlottedAnchorPositionFinder(slotHandler);
        activeAnchorState = slottedAnchorState;
            
    }

    private void OnEnable()
    {
        inputHandler.OnDragBegin += OnDragBegin;
        inputHandler.OnDragEnd += OnDragEnd;
        inputHandler.OnDragChange += OnDrag;
    }

    private void OnDisable()
    {
        inputHandler.OnDragBegin -= OnDragBegin;
        inputHandler.OnDragEnd -= OnDragEnd;
        inputHandler.OnDragChange -= OnDrag;
    }

    private Vector2 dragOriginOffset;
    private void OnDragBegin(Vector2 mousePosition)
    {
        dragOriginOffset = anchorHandler.Origin() - mousePosition;
        
        zoneHandler.OnNearestZoneChange += OnNearestZoneUpdate;
        inputHandler.ToggleSelect -= ToggleSelectState;
        
        activeAnchorState = draggingAnchorState;
    }

    private void OnDragEnd(Vector2 mousePosition)
    {
        zoneHandler.OnNearestZoneChange -= OnNearestZoneUpdate;
        inputHandler.ToggleSelect += ToggleSelectState;
        
        activeAnchorState = slottedAnchorState;
    }

    private void OnDrag(Vector2 mousePosition)
    {

        (draggingAnchorState as DraggingAnchorPositionFinder)?.SetDragPos(mousePosition + dragOriginOffset);

        zoneHandler.Search(anchorHandler.Origin());
        
        slotHandler.ReorderActiveZone(anchorHandler.Origin(), this);
    }

    private void Update()
    {
        anchorHandler.Follow(activeAnchorState.GetFollowPosition());
        displayHandler.Follow(anchorHandler.Origin(), rot);
    }

    private void OnNearestZoneUpdate(IZone zone)
    {
        slotHandler.UpdateActiveZone(zone, this);
    }

    private bool selected;
    [SerializeField] private float selectedHeight = 4;
    private void ToggleSelectState()
    {
        selected = !selected;
        anchorHandler.SetOffset(Vector2.up * (selected ? selectedHeight : 0));
        displayHandler.SetSelected(selected);
    }

    public Transform GetZoneTransform() => slotHandler.Transform();

    public Transform GetDisplayTransform() => displayHandler.Transform();
    private float rot;

    public void SetRotation(float rotation)
    {
        rot = rotation;
    }

}

public interface IZonable
{
    public Transform GetZoneTransform();
    public Transform GetDisplayTransform();
    public void SetRotation(float rotation);

}

public class DraggingAnchorPositionFinder : IAnchorFollowFinder
{
    public void SetDragPos(Vector2 pos)
    {
        this.pos = pos;
    }

    private Vector2 pos;
    public Vector2 GetFollowPosition()
    {
        return pos;
    }
}

public class SlottedAnchorPositionFinder : IAnchorFollowFinder
{
    private Transform slotPos;
    public SlottedAnchorPositionFinder(ICardSlot slot)
    {
        slotPos = slot.Transform();
    }
    public Vector2 GetFollowPosition()
    {
        return slotPos.position;
    }
}

public interface IAnchorFollowFinder
{
    public Vector2 GetFollowPosition();
}