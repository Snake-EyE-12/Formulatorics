using UnityEngine;

public class CardBuilder : CardBuildable
{
    private CardController _prefab;
    private Transform _parent;
    private Vector3 _position;
    private Quaternion _rotation;

    public CardBuilder(CardController prefab)
    {
        _prefab = prefab;
    }

    public CardController Build()
    {
        return null;//GameObject.Instantiate(_prefab, _position, _rotation, _parent);
    }
    
    

    public CardBuildable WithParent(Transform parent)
    {
        _parent = parent;
        return this;
    }

    public CardBuildable WithPosition(Vector3 position)
    {
        _position = position;
        return this;
    }

    public CardBuildable WithRotation(Quaternion rotation)
    {
        _rotation = rotation;
        return this;
    }
}


public interface CardBuildable
{
    public CardController Build();
    public CardBuildable WithParent(Transform parent);
    public CardBuildable WithPosition(Vector3 position);
    public CardBuildable WithRotation(Quaternion rotation);
}