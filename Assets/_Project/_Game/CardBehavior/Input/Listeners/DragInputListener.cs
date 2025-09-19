using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInputListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ICardDragInputReceiver dragInputReceiver;
    private void Awake()
    {
        dragInputReceiver = gameObject.GetComponent<ICardDragInputReceiver>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragInputReceiver.StartDrag(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragInputReceiver.Drag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragInputReceiver.StopDrag(eventData.position);
    }
}