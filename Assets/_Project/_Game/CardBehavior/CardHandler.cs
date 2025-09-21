using System;
using System.Collections.Generic;
using System.Linq;
//using Cobra.Utilities.Extensions;
using UnityEngine;

public class CardHandler : MonoBehaviour, IZonable, IHoverable, IFocusable, IDraggable, ISelectable, IShowcaseable
{
    #region Initialization
    
    private IZoneSlot slotHandler;
    private ICardAnchor anchorHandler;
    private ICardDisplay displayHandler;
    private ICardInputReader inputHandler;
    private ICardZoneFinder zoneHandler;
    
    private void Awake()
    {
        slotHandler = GetComponentInChildren<IZoneSlot>();
        anchorHandler = GetComponentInChildren<ICardAnchor>();
        displayHandler = GetComponentInChildren<ICardDisplay>();
        inputHandler = GetComponentInChildren<ICardInputReader>();
        zoneHandler = GetComponent<ICardZoneFinder>();
    }

    private void Start()
    {
        inputHandler.OnClicked += OnClickPerformed;
        inputHandler.OnHoverEnter += HoverEnterPerformed;
        inputHandler.OnHoverExit += HoverExitPerformed;
        inputHandler.OnDragBegin += DragStartPerformed;
        inputHandler.OnDragEnd += DragEndPerformed;
        inputHandler.OnDragChange += DragPerformed;
    }
    
    #endregion

    #region Selection
    private bool currentlySelected;
    private void OnClickPerformed()
    {
        if(!currentlySelected) ServiceLocator.Get<ISelectControl>().Select(this);
        else ServiceLocator.Get<ISelectControl>().Deselect(this);
    }

    [SerializeField] private float selectionHeight;
    public void OnSelect()
    {
        currentlySelected = true;
        slotHandler.SetOffset(Vector2.up * selectionHeight);
    }

    public void OnDeselect()
    {
        currentlySelected = false;
        slotHandler.SetOffset(Vector2.zero);
    }

    public void OnFailSelect()
    {
        // do little flick
    }

    #endregion

    #region Hover

    private void HoverEnterPerformed()
    {
        ServiceLocator.Get<IHoverControl>().HoverEnter(this);
    }
    private void HoverExitPerformed()
    {
        ServiceLocator.Get<IHoverControl>().HoverExit(this);
    }

    [SerializeField] private float growthPercent;
    public void OnGainHover()
    {
        // input size expand
        inputHandler.Grow(growthPercent);
        // display size expand slow
        displayHandler.Grow(growthPercent);
        // display flick animation
        ServiceLocator.Get<IFocusControl>().Focus(this);
        ServiceLocator.Get<IViewControl>().Showcase(this);
    }

    public void OnLostHover()
    {
        // input size shrink
        inputHandler.Shrink();
        // display size shrink
        displayHandler.Shrink();
        // display small flick animation
        ServiceLocator.Get<IViewControl>().Conceal(this);
    }
    
    #endregion
    
    #region Dragging

    private void DragStartPerformed(Vector2 pos)
    {
        ServiceLocator.Get<IDragControl>().StartDrag(this, pos);
    }
    private void DragEndPerformed(Vector2 pos)
    {
        ServiceLocator.Get<IDragControl>().EndDrag(this, pos);
    }
    private void DragPerformed(Vector2 pos)
    {
        ServiceLocator.Get<IDragControl>().Drag(this, pos);
    }
    

    private Vector2 dragOriginOffset;
    private bool dragging;
    public void OnDragBegin(Vector2 mousePosition)
    {
        dragging = true;
        dragOriginOffset = inputHandler.Origin() - mousePosition;
        zoneHandler.OnNearestZoneChange += OnNearestZoneUpdate;
    }

    public void OnDragEnd(Vector2 mousePosition)
    {
        dragging = false;
        zoneHandler.OnNearestZoneChange -= OnNearestZoneUpdate;
    }

    public void OnDrag(Vector2 mousePosition)
    {
        anchorHandler.Follow(mousePosition + dragOriginOffset);
        
        zoneHandler.Search(anchorHandler.Origin());
        
        slotHandler.ReorderActiveZone(anchorHandler.Origin(), this);
    }
    
    #endregion
    
    #region Focus
    
    public void OnShowcase()
    {
        
    }

    public void OnConceal()
    {
        displayHandler.SetParent(currentZoneDisplayParent);
        inputHandler.SetParent(currentZoneInputBoxParent);
        SetFullSiblingOrder(previousSiblingIndex);
    }
    public void OnGainFocus() {}

    public void OnLoseFocus() {}
    
    private Transform currentZoneDisplayParent;
    private Transform currentZoneInputBoxParent;
    public void SetVisualParentAs(Transform parent) => displayHandler.SetParent(parent);
    public void SetInputParentAs(Transform parent) => inputHandler.SetParent(parent);
    #endregion
    
    #region Zoning
    
    private void OnNearestZoneUpdate(IZone zone)
    {
        slotHandler.UpdateActiveZone(zone, this);
    }

    public void SlotTo(Vector2 position, float angle, int siblingIndex)
    {
        slotHandler.SlotTo(position, angle);
        previousSiblingIndex = siblingIndex;
        SetFullSiblingOrder(siblingIndex);
    }

    private int previousSiblingIndex = 0;
    private void SetFullSiblingOrder(int siblingIndex)
    {
        slotHandler.SetSiblingOrder(siblingIndex);
        displayHandler.SetSiblingOrder(siblingIndex);
        inputHandler.SetSiblingOrder(siblingIndex);
    }

    public void OnZoneJoined(Transform slotParent, Transform displayParent, Transform inputBoxParent)
    {
        slotHandler.SetParent(slotParent);
        currentZoneDisplayParent = displayParent;
        currentZoneInputBoxParent = inputBoxParent;
    }

    
    #endregion

    private void Update()
    {
        if(!dragging) anchorHandler.Follow(slotHandler.Origin());
        displayHandler.Follow(anchorHandler.Origin(), slotHandler.Angle());
        inputHandler.Follow(anchorHandler.Origin(), slotHandler.Angle());
    }

}

public interface IZonable
{
    public void SlotTo(Vector2 position, float angle, int siblingIndex);
    public void OnZoneJoined(Transform slotParent, Transform displayParent, Transform inputBoxParent);
}

public interface IFocusable
{
    public void OnGainFocus();
    public void OnLoseFocus();
}

public interface IShowcaseable : IHasVisual, IHasInputBox
{
    public void OnShowcase();
    public void OnConceal();
}

public interface IHasVisual
{
    public void SetVisualParentAs(Transform parent);
}

public interface IHasInputBox
{
    public void SetInputParentAs(Transform parent);
}

public interface IHoverable
{
    public void OnGainHover();
    public void OnLostHover();
}

public interface IDraggable
{
    public void OnDragBegin(Vector2 position);
    public void OnDragEnd(Vector2 position);
    public void OnDrag(Vector2 position);
}

public interface ISelectable
{
    public void OnSelect();
    public void OnDeselect();
    public void OnFailSelect();
}

public interface ILayerOrderable
{
    public void SetParent(Transform newParent);
    public void SetSiblingOrder(int siblingIndex);
}