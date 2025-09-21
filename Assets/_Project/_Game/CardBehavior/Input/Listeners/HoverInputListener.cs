using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInputListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ICardHoverInputReceiver hoverInput;
    private void Awake()
    {
        hoverInput = gameObject.GetComponent<ICardHoverInputReceiver>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverInput.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverInput.OnPointerExit();
    }
}