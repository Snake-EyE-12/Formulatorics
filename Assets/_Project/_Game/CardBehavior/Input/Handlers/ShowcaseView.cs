using UnityEngine;

public class ShowcaseView : MonoBehaviour, IShowcase
{
    [SerializeField] private Transform inputContainer;
    [SerializeField] private Transform visualContainer;
    
    public void Present(IShowcaseable child)
    {
        child.SetVisualParentAs(visualContainer);
        child.SetInputParentAs(inputContainer);
    }
}

public interface IShowcase
{
    public void Present(IShowcaseable child);
}