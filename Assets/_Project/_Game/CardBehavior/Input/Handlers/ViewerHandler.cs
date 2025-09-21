using UnityEngine;

public class ViewerHandler : MonoBehaviour, IViewControl
{
    private void Awake()
    {
        showcase = GetComponent<IShowcase>();
        ServiceLocator.Register<IViewControl>(this);
    }

    private IShowcaseable item;
    private IShowcase showcase;

    public void Showcase(IShowcaseable candidate)
    {
        item?.OnConceal();
        item = candidate;
        candidate.OnShowcase();
        showcase.Present(candidate);
    }

    public void Conceal(IShowcaseable candidate)
    {
        if (candidate != item) return;
        item = null;
        candidate.OnConceal();
    }
}

public interface IViewControl : IService
{
    public void Showcase(IShowcaseable candidate);
    public void Conceal(IShowcaseable candidate);
}