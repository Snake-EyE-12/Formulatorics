using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay
{
    [SerializeField] private float translationSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    private ICardFollowStrategy followStrategy;
    [SerializeField] private Image image;

    private void Awake()
    {
        followStrategy = GetComponent<ICardFollowStrategy>();
        image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        initShadowPos = shadow.anchoredPosition;
    }

    public void Follow(Vector2 posTarget, float angleTarget)
    {
        transform.position = followStrategy.Position(transform.position, posTarget, translationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angleTarget), rotationSpeed * Time.deltaTime);
    }

    public Transform Transform() => transform;
    [SerializeField] private RectTransform shadow;
    private Vector2 initShadowPos;
    [SerializeField] private Vector2 shadowOffsetActive;
    public void SetSelected(bool active)
    {
        if (active)
        {
            transform.localScale = Vector3.one * 1.4f;
            shadow.anchoredPosition = initShadowPos + shadowOffsetActive;
        }
        else
        {
            transform.localScale = Vector3.one * 1.0f;
            shadow.anchoredPosition = initShadowPos;
        }
    }
}

public interface ICardDisplay
{
    public void Follow(Vector2 posTarget, float angleTarget);
    public Transform Transform();
    public void SetSelected(bool active);
}