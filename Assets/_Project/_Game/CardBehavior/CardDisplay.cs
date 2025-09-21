using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay
{
    [SerializeField] private float translationSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    private FollowTranslationalStrategy followTranslationalStrategy;
    private FollowRotationalStrategy followRotationalStrategy;
    [SerializeField] private Image image;

    private void Awake()
    {
        followTranslationalStrategy = GetComponent<FollowTranslationalStrategy>();
        followRotationalStrategy = GetComponent<FollowRotationalStrategy>();
        image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        initialScale = transform.localScale;
        desiredScale = initialScale;
    }

    public void Follow(Vector2 posTarget, float angleTarget)
    {
        transform.position = followTranslationalStrategy.CalculatePosition(transform.position, posTarget);
        transform.rotation = followRotationalStrategy.CalculateRotation(transform.rotation, Quaternion.Euler(0, 0, angleTarget));
    }

    [SerializeField] private RectTransform shadow;

    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    public void SetSiblingOrder(int siblingIndex)
    {
        transform.SetSiblingIndex(siblingIndex);
    }

    private Vector3 initialScale;
    private Vector3 desiredScale;
    public void Grow(float percent)
    {
        desiredScale = initialScale * percent;
    }

    [SerializeField] private float scaleSpeed = 10f;

    public void Shrink()
    {
        desiredScale = initialScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * scaleSpeed);
    }
}

public interface ICardDisplay : ILayerOrderable, ISizable
{
    public void Follow(Vector2 posTarget, float angleTarget);
}

public interface ISizable
{
    public void Grow(float percent);
    public void Shrink();
}