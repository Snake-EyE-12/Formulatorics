using UnityEngine;

public class CardSelectionControllerConcrete : MonoBehaviour, CardSelectionController
{
    private int count;
    private int maxSelect = 4;
    public bool TrySelect(CardSelectable selectable)
    {
        if (maxSelect >= count)
        {
            selectable.FailSelect();
            return false;
        }

        count++;
        selectable.Select();
        return true;
    }

    public bool TryDeselect(CardSelectable selectable)
    {
        count--;
        selectable.Deselect();
        return true;
    }
}


public interface CardSelectionController
{
    public bool TrySelect(CardSelectable selectable);
    public bool TryDeselect(CardSelectable selectable);
}