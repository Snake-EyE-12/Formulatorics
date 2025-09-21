using System;
using Cobra.DesignPattern;using UnityEngine;

public class SelectHandler : MonoBehaviour, ISelectControl
{
    private void Awake()
    {
        ServiceLocator.Register<ISelectControl>(this);
    }
    

    // List of selections
    // Rule for total
    // Call fail in case
    public void Select(ISelectable selectable)
    {
        Debug.Log(selectable);
        selectable.OnSelect();
    }

    public void Deselect(ISelectable selectable)
    {
        selectable.OnDeselect();
    }
}

public interface ISelectControl : IService
{
    public void Select(ISelectable selectable);
    public void Deselect(ISelectable selectable);
}