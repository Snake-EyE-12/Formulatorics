using Cobra.Utilities;
using UnityEngine;

public class CardDisplayEffectShadow : MonoBehaviour
{
    private float initialYOffset;
    [SerializeField] private Curve xOffsetCurve;
    private FollowTranslationalStrategy followTranslationalStrategy;

    private void Awake()
    {
        initialYOffset = transform.localPosition.y;
        followTranslationalStrategy = GetComponent<FollowTranslationalStrategy>();
    }

    private void Update()
    {
        float followedY = followTranslationalStrategy.CalculatePosition(new Vector3(0, transform.localPosition.y, 0), new Vector3(0, initialYOffset + yOffset, 0)).y;
        transform.localPosition = new Vector3(GetXOffset(), followedY, 0);
    }

    private float yOffset;
    [SerializeField] private float offsetOnDrag = -20;
    public void SetYOffset()
    {
        yOffset = offsetOnDrag;
    }

    public void ResetYOffset()
    {
        yOffset = 0;
    }

    private float GetXOffset()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        float uvX = screenPos.x / Screen.width;
        return xOffsetCurve.Evaluate(uvX);
    }

}