using UnityEngine;
using UnityEngine.EventSystems;

public class PressInputListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ICardPressInputReceiver pressInput;
    private void Awake()
    {
        pressInput = gameObject.GetComponent<ICardPressInputReceiver>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressInput.OnDownPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressInput.OnUpPressed();
    }
}