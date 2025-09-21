using System;
using UnityEngine;

public class CardAnchor : MonoBehaviour, ICardAnchor
{
    private FollowTranslationalStrategy followTranslationalStrategy;

    private void Awake()
    {
        followTranslationalStrategy = GetComponent<FollowTranslationalStrategy>();
    }

    public void Follow(Vector2 target)
    {
        transform.position = followTranslationalStrategy.CalculatePosition(transform.position, target);
    }
    public Vector2 Origin() => transform.position;
}

public interface ICardAnchor : IPositionFollowable
{
    public void Follow(Vector2 position);
}

public interface IPositionFollowable
{
    public Vector2 Origin();
}

public interface IRotationFollowable
{
    public float Angle();
}