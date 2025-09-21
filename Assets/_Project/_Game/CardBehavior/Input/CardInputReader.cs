using System;
using UnityEngine;

public class CardInputReceiver : MonoBehaviour, ICardPressInputReceiver, ICardHoverInputReceiver, ICardDragInputReceiver, ICardInputReader
{
    #region Press Control
    private bool pressed = false;
    public void OnDownPressed()
    {
        pressed = true;
    }

    public void OnUpPressed()
    {
        if(pressed) OnClicked?.Invoke();
        pressed = false;
    }

    public Action OnClicked { get; set; }

    #endregion
    
    #region Hover Control

    public void OnPointerEnter()
    {
        OnHoverEnter?.Invoke();
    }

    public void OnPointerExit()
    {
        pressed = false;
        OnHoverExit?.Invoke();
    }
    
    public Action OnHoverEnter { get; set; }
    public Action OnHoverExit { get; set; }
    
    #endregion

    #region Drag Control
    public void StartDrag(Vector2 position)
    {
       OnDragBegin?.Invoke(position);
       pressed = false;
    }
    public void StopDrag(Vector2 position)
    {
        OnDragEnd?.Invoke(position);
    }

    public void Drag(Vector2 position)
    {
        OnDragChange?.Invoke(position);
    }

    public Action<Vector2> OnDragBegin { get; set; }
    public Action<Vector2> OnDragEnd { get; set; }
    public Action<Vector2> OnDragChange { get; set; }

    #endregion


    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    public void SetSiblingOrder(int siblingIndex)
    {
        transform.SetSiblingIndex(siblingIndex);
    }

    private void Awake()
    {
        translationStrategy = GetComponent<FollowTranslationalStrategy>();
        rotationalStrategy = GetComponent<FollowRotationalStrategy>();
        initialScale = transform.localScale;
    }

    private FollowTranslationalStrategy translationStrategy;
    private FollowRotationalStrategy rotationalStrategy;
    public void Follow(Vector2 posTarget, float angleTarget)
    {
        transform.position = translationStrategy.CalculatePosition(transform.position, posTarget);
        transform.rotation = rotationalStrategy.CalculateRotation(transform.rotation, Quaternion.Euler(0, 0, angleTarget));
    }
    
    public Vector2 Origin()
    {
        return transform.position;
    }
    
    private Vector3 initialScale;
    public void Grow(float percent)
    {
        transform.localScale = initialScale * percent;
    }

    public void Shrink()
    {
        transform.localScale = initialScale;
    }

}

public interface ICardInputReader : ILayerOrderable, IPositionFollowable, ISizable
{
    public void Follow(Vector2 posTarget, float angleTarget);
    public Action<Vector2> OnDragBegin { get; set; }
    public Action<Vector2> OnDragEnd { get; set; }
    public Action<Vector2> OnDragChange { get; set; }
    public Action OnClicked { get; set; }
    public Action OnHoverEnter { get; set; }
    public Action OnHoverExit { get; set; }
}
public interface ICardDragInputReceiver
{
    public void StartDrag(Vector2 position);
    public void StopDrag(Vector2 position);
    public void Drag(Vector2 position);
}

public interface ICardHoverInputReceiver
{
    public void OnPointerEnter();
    public void OnPointerExit();
}

public interface ICardPressInputReceiver
{
    public void OnDownPressed();
    public void OnUpPressed();

}