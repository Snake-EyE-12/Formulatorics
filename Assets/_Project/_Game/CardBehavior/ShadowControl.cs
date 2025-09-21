using Cobra.Utilities;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    private Vector2 initialOffset;
    private float variableOffset;
    [SerializeField] private Curve xOffsetCurve;

    private void Awake()
    {
        initialOffset = transform.localPosition;
    }

    private void Update()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        float uvX = screenPos.x / Screen.width;
        float localX = xOffsetCurve.Evaluate(uvX);
        Vector2 target = initialOffset + new Vector2(localX, variableOffset);
        float t = 1f - Mathf.Exp(-10 * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, t);
    }
    

    public void SetOffset(float offset)
    {
        variableOffset = offset;
    }
}