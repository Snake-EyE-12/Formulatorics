using System;
using Cobra.Utilities.Extensions;
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
        pressed = false;
        ToggleSelect?.Invoke();
    }

    public Action ToggleSelect { get; set; }

    #endregion
    
    #region Hover Control
    private bool hovering;
    public void OnHoverEnter()
    {
        hovering = true;
    }

    public void OnHoverExit()
    {
        hovering = false;
    }
    #endregion

    #region Drag Control
    public void StartDrag(Vector2 position)
    {
       OnDragBegin?.Invoke(position);
    }
    public void StopDrag(Vector2 position)
    {
        OnDragEnd?.Invoke(position);
    }

    public void Drag(Vector2 position)
    {
        OnDragChange?.Invoke(position);
    }

    #endregion

    public Action<Vector2> OnDragBegin { get; set; }
    public Action<Vector2> OnDragEnd { get; set; }
    public Action<Vector2> OnDragChange { get; set; }
}

public interface ICardInputReader
{
    public Action<Vector2> OnDragBegin { get; set; }
    public Action<Vector2> OnDragEnd { get; set; }
    public Action<Vector2> OnDragChange { get; set; }
}
public interface ICardDragInputReceiver
{
    public void StartDrag(Vector2 position);
    public void StopDrag(Vector2 position);
    public void Drag(Vector2 position);
}

public interface ICardHoverInputReceiver
{
    public void OnHoverEnter();
    public void OnHoverExit();
}

public interface ICardPressInputReceiver
{
    public void OnDownPressed();
    public void OnUpPressed();

}