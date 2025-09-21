using System;
using System.Collections.Generic;
using Cobra.DesignPattern;using UnityEngine;

public class FocusHandler : MonoBehaviour, IFocusControl
{
    private void Awake()
    {
        ServiceLocator.Register<IFocusControl>(this);
    }

    private IFocusable previous;
    private IFocusable inFocus;

    public void Focus(IFocusable candidate)
    {
        inFocus?.OnLoseFocus();
        previous = candidate;
        inFocus = candidate;
        candidate.OnGainFocus();
    }

    public void Defocus(IFocusable candidate)
    {
        if (candidate != inFocus) return;
        inFocus = null;
        candidate.OnLoseFocus();
    }

    public IFocusable RetrieveLastFocus()
    {
        if(previous != null) return previous;
        return GetStartingFocalPoint();
    }
    private IFocusable GetStartingFocalPoint()
    {
        return null; //TODO: Give something starting focus for shortcut retrieval
    }
    public void ClearFocalHistory() => previous = null;
}

public interface IFocusControl : IService
{
    public void Focus(IFocusable candidate);
    public void Defocus(IFocusable candidate);
    public IFocusable RetrieveLastFocus();
    public void ClearFocalHistory();
}