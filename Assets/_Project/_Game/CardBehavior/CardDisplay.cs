using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay
{
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
        shadow.SetOffset(shadowOffset);
    }

    [SerializeField] private float scaleSpeed = 10f;
    [SerializeField] private ShadowControl shadow;
    [SerializeField] private float shadowOffset = 20f;

    public void Shrink()
    {
        desiredScale = initialScale;
        shadow.SetOffset(0);
    }

    private void Update()
    {
        float t = 1f - Mathf.Exp(-scaleSpeed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, t);
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