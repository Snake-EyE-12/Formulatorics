using UnityEngine;
using UnityEngine.EventSystems;

public class PressInputReceiver : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ICardPressInputReader pressInput;
    private void Awake()
    {
        pressInput = gameObject.GetComponent<ICardPressInputReader>();
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