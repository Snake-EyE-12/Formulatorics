using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay
{
    [SerializeField] private float speed = 10f;
    private ICardFollowStrategy followStrategy;

    private void Awake()
    {
        followStrategy = GetComponent<ICardFollowStrategy>();
        GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    public void Follow(Vector2 target)
    {
        transform.position = followStrategy.Position(transform.position, target, speed);
    }

    public Transform Transform() => transform;
}

public interface ICardDisplay
{
    public void Follow(Vector2 target);
    public Transform Transform();
}