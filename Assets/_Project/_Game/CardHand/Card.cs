using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform gfx;
    private bool selected = false;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination.position + (Vector3.up * (selected ? 20 : 0)), Time.deltaTime * 10);
        transform.rotation = Quaternion.Lerp(transform.rotation, destination.rotation, Time.deltaTime * 10);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Scale Up
        transform.localScale = Vector3.one * 1.2f;
        //Bring to front
        gfx.localPosition = new Vector3(0, 0, 1);
        //Rotate to mouse
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Scale Down
        transform.localScale = Vector3.one;
        //Bring to normal
        gfx.localPosition = new Vector3(0, 0, 0);
        //Rotate randomly again
    }
}
