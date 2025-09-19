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
        transform.position = followStrategy.Position(transform.position, target, 0);
    }
    public Vector2 Origin() => transform.position;
}

public interface ICardAnchor
{
    public void Follow(Vector2 position);
    public Vector2 Origin();
}
