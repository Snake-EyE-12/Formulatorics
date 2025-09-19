using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInputReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ICardHoverInputReader hoverInput;
    private void Awake()
    {
        hoverInput = gameObject.GetComponent<ICardHoverInputReader>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverInput.OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverInput.OnHoverExit();
    }
}