using UnityEngine;

public class CardController : MonoBehaviour
{
    private ICardHoverController hoverController;
    private ICardSelectionController selectionController;
    private ICardDragController dragController;
    private ICardZoneController zoneController;
}


public interface CardSelectable
{
    public void Select();
    public void Deselect();
    public void FailSelect();
    public bool IsSelected();
}

public class CardHoverController : ICardHoverController
{
    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        
    }
}
public interface ICardHoverController
{
    public void OnHoverEnter();
    public void OnHoverExit();
}

public interface ICardSelectionController
{
    public void OnSelect();
    public void OnDeselect();
}

public interface ICardDragController
{
    public void OnDragEnter();
    public void OnDragExit();
}

public interface ICardZoneController
{
    public void OnZoneEnter();
    public void OnZoneExit();
}