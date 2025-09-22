using Cobra.DesignPattern;
using UnityEngine;

public class HoverHandler : MonoBehaviour, IHoverControl
{
    private void Awake()
    {
        ServiceLocator.Register<IHoverControl>(this);
    }
    
    private IHoverable previous;
    
    public void HoverEnter(IHoverable candidate)
    {
        if (candidate == previous) return;
        previous?.OnLostHover();
        previous = candidate;
        candidate.OnGainHover();
        ServiceLocator.Get<IGroupInteractionControl>().OnHoveredOverSomething();
    }

    public void HoverExit(IHoverable candidate)
    {
        if (candidate != previous) return;
        previous?.OnLostHover();
        previous = null;
        ServiceLocator.Get<IGroupInteractionControl>().OnHoveringNothing();
    }
}

public interface IHoverControl : IService
{
    public void HoverEnter(IHoverable candidate);
    public void HoverExit(IHoverable candidate);
}