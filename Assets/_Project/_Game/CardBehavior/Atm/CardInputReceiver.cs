using UnityEngine;
using UnityEngine.EventSystems;

public class CardInputReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ICardHoverHandler hoverHandler;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverHandler.OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverHandler.OnHoverExit();
    }
}

public interface ICardHoverHandler
{
    public void OnHoverEnter();
    public void OnHoverExit();
}