using System;
using Cobra.Utilities.Extensions;
using UnityEngine;

public class CardAnchor : MonoBehaviour, ICardAnchor
{
    private ICardFollowStrategy followStrategy;

    private void Awake()
    {
        followStrategy = GetComponent<ICardFollowStrategy>();
    }

    public void Follow(Vector2 target)
    {
        transform.position = followStrategy.Position(transform.position, target + heightOffset, 0);
    }
    public Vector2 Origin() => transform.position;
    private Vector2 heightOffset;
    public void SetOffset(Vector2 offset)
    {
        heightOffset = offset;
    }
}

public interface ICardAnchor
{
    public void Follow(Vector2 position);
    public Vector2 Origin();
    public void SetOffset(Vector2 offset);
}
