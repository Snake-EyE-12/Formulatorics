using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInputReceiver : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ICardDragInputReader dragInput;
    private void Awake()
    {
        dragInput = gameObject.GetComponent<ICardDragInputReader>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragInput.StartDrag(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragInput.Drag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragInput.StopDrag(eventData.position);
    }
}