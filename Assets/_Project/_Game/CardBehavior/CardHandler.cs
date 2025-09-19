using System;
using System.Collections.Generic;
using System.Linq;
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
        
        activeAnchorState = draggingAnchorState;
    }

    private void OnDragEnd(Vector2 mousePosition)
    {
        zoneHandler.OnNearestZoneChange -= OnNearestZoneUpdate;
        
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
        displayHandler.Follow(anchorHandler.Origin());
    }

    private void OnNearestZoneUpdate(IZone zone)
    {
        slotHandler.UpdateActiveZone(zone, this);
    }

    public Transform GetZoneTransform() => slotHandler.Transform();

    public Transform GetDisplayTransform() => displayHandler.Transform();
}

public interface IZonable
{
    public Transform GetZoneTransform();
    public Transform GetDisplayTransform();
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