using Cobra.Utilities;
using Cobra.Utilities.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GimbalState
{
    Idle,
    Mouse,
    Velocity
}
public class CardDisplayEffectGimbal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [Header("Drag")]
    [SerializeField] private float tolerance = 1;
    [SerializeField] private Curve xVelToAngle;
    [Header("Hover")]
    [SerializeField] private Curve pressureCurve;
    [Header("Idle")]
    [SerializeField] private Curve spinCurve;
    [SerializeField] private float spinSpeed = 0.1f;

    private FollowRotationalStrategy rotationalStrategy;
    private GimbalState state;

    private void Awake()
    {
        rotationalStrategy = GetComponent<FollowRotationalStrategy>();
    }

    private Vector2 previousPosition;
    private void Update()
    {
        transform.localRotation = rotationalStrategy.CalculateRotation(transform.localRotation, GetTargetRotation());
        previousPosition = transform.position;
    }

    private Quaternion GetTargetRotation()
    {
        switch (state)
        {
            case GimbalState.Mouse:
                return GetMouseHoverPressureAngle();
            case GimbalState.Velocity:
                return GetVelocityAngle();
            case GimbalState.Idle:
            default:
                return GetSignedFloatingAngle();
        }
    }
    
    public void FollowMouse()
    {
        state = GimbalState.Mouse;
    }

    public void Gyrate()
    {
        state = GimbalState.Idle;
    }

    public void FollowVelocity()
    {
        state = GimbalState.Velocity;
    }

    private Quaternion GetVelocityAngle()
    {
        if(Vector2.SqrMagnitude(transform.position.xy() - previousPosition) < tolerance) return Quaternion.identity;
        Vector2 moveDir = transform.position.xy() - previousPosition;
        return Quaternion.Euler(0, 0, xVelToAngle.Evaluate(moveDir.x));
    }
    private Vector2 GetHoverLocation()
    {
        return Mouse.current.position.value;
    }

    
    private float timeOffset;
    private float TimeOffset => Time.time + timeOffset;
    public void RandomizeTiming(float t)
    {
        timeOffset = t;
    }
    private Quaternion GetSignedFloatingAngle()
    {
        return Quaternion.Euler(
            spinCurve.Evaluate(0.5f * (Mathf.Sin(TimeOffset * spinSpeed) + 1)),
            -spinCurve.Evaluate(0.5f * (Mathf.Cos(TimeOffset * spinSpeed) + 1)),
            0
            );
    }
    private Quaternion GetMouseHoverPressureAngle()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            GetHoverLocation(),
            null,
            out Vector2 localPos
        );

        Rect rect = rectTransform.rect;

        float u = Mathf.InverseLerp(rect.xMin, rect.xMax, localPos.x);
        float v = Mathf.InverseLerp(rect.yMin, rect.yMax, localPos.y);

        Vector2 mouseUVBasedOnContextRect = new Vector2(u, v);

        return Quaternion.Euler(
            pressureCurve.Evaluate(mouseUVBasedOnContextRect.y),
            -pressureCurve.Evaluate(mouseUVBasedOnContextRect.x),
            0);
    }

}