using Cobra.Utilities;
using UnityEngine;

public class ShadowOffsetCalculator : MonoBehaviour, IShadowXOffsetFinder
{
    [SerializeField] private Curve xOffsetCurve;

    public float GetOffset()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        float uvX = screenPos.x / Screen.width;
        return xOffsetCurve.Evaluate(uvX);
    }
}

public interface IShadowXOffsetFinder
{
    public float GetOffset();
}