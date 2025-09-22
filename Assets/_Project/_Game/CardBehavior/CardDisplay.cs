using System;
using Cobra.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay, ICardDisplayStateContext
{
    private FollowTranslationalStrategy followTranslationalStrategy;
    private FollowRotationalStrategy followRotationalStrategy;
    
    [SerializeField] private HoveringDisplayState hoveringState;
    [SerializeField] private DraggingDisplayState draggingState;
    [SerializeField] private IdlingDisplayState idleState;

    private IShadowXOffsetFinder shadowOffsetFinder;
    [SerializeField] private Transform shadow;
    private Vector3 shadowInitialOffset;
    private Vector3 shadowDesiredOffset;
    
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text textbox;

    private void Awake()
    {
        followTranslationalStrategy = GetComponent<FollowTranslationalStrategy>();
        followRotationalStrategy = GetComponent<FollowRotationalStrategy>();
        shadowOffsetFinder = GetComponent<IShadowXOffsetFinder>();
        ChangeState(idleState);
        SetUpTempColor();
        InitialScale = transform.localScale;
        DesiredScale = InitialScale;
        shadowInitialOffset = shadow.localPosition;
        shadowDesiredOffset = shadowInitialOffset;
    }

    private void SetUpTempColor()
    {
        image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private ICardDisplayState activeState;
    private void ChangeState(ICardDisplayState newState)
    {
        activeState?.Exit(this);
        activeState = newState;
        activeState?.Enter(this);
        textbox.text = newState.GetType().Name;
    }

    private Vector2 posOffset;
    private Quaternion angleOffset;
    private Vector2 targetPos;
    private Quaternion targetAngle;

    public void Target(Vector2 posTarget, float angleTarget)
    {
        targetPos = posTarget;
        targetAngle = Quaternion.Euler(0, 0, angleTarget);
    }


    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    public void SetSiblingOrder(int siblingIndex)
    {
        transform.SetSiblingIndex(siblingIndex);
    }


    private bool hovering = false;
    public void OnHover()
    {
        hovering = true;
        ChangeState(hoveringState);
    }
    public void OnHoverExit()
    {
        hovering = false;
        ChangeState(idleState);
    }

    private float scalePercent;
    public void OnStartDrag(float percent)
    {
        ChangeState(draggingState);
        scalePercent = percent;
    }

    public void OnEndDrag()
    {
        if(hovering) ChangeState(hoveringState);
        else ChangeState(idleState);
    }

    [SerializeField] private float scaleSpeed = 10f;
    private void Update()
    {
        activeState?.Update(this);
    }

    public void ApproachDestination()
    {
        transform.position = followTranslationalStrategy.CalculatePosition(transform.position, targetPos + posOffset);
        transform.rotation = followRotationalStrategy.CalculateRotation(transform.rotation, targetAngle * angleOffset);
        float t = 1f - Mathf.Exp(-scaleSpeed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, DesiredScale, t);
        shadow.transform.localPosition = Vector3.Lerp(shadow.transform.localPosition, shadowDesiredOffset + (Vector3.right * shadowOffsetFinder.GetOffset()), t);
    }

    [field:SerializeField] public RectTransform RectTransform { get; private set; }
    public Vector2 GetHoverLocation()
    {
        return Mouse.current.position.value;
    }

    public void SetAngleOffset(Quaternion angleOffset)
    {
        this.angleOffset = angleOffset;
    }

    public void SetPositionOffset(Vector2 offset)
    {
        posOffset = offset;
    }

    public Vector3 InitialScale { get; private set; }
    public Vector3 DesiredScale { get; set; }
    public void SizeUp()
    {
        shadowDesiredOffset = shadowInitialOffset + (Vector3.up * shadowOffsetAmount);
        DesiredScale = InitialScale * scalePercent;
    }

    [SerializeField] private float shadowOffsetAmount = -20;

    public void SizeDown()
    {
        shadowDesiredOffset = shadowInitialOffset;
        DesiredScale = InitialScale;
    }
}
[Serializable]
public class HoveringDisplayState : ICardDisplayState
{
    public void Enter(ICardDisplayStateContext context)
    {
    }

    public void Update(ICardDisplayStateContext context)
    {
        Vector2 mouseScreen = context.GetHoverLocation();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            context.RectTransform,
            mouseScreen,
            null,
            out Vector2 localPos
        );
        Rect rect = context.RectTransform.rect;
        float u = Mathf.InverseLerp(rect.xMin, rect.xMax, localPos.x);
        float v = Mathf.InverseLerp(rect.yMin, rect.yMax, localPos.y);
        Vector2 mouseUVBasedOnContextRect = new Vector2(u, v);

        context.SetAngleOffset(UVToAngle(mouseUVBasedOnContextRect));
        context.ApproachDestination();
    }

    
    [SerializeField] private Curve xHoverRotationCurve;
    [SerializeField] private Curve yHoverRotationCurve;
    private Quaternion UVToAngle(Vector2 mouseUV)
    {
        return Quaternion.Euler(
            xHoverRotationCurve.Evaluate(mouseUV.x),
            yHoverRotationCurve.Evaluate(mouseUV.y),
            0
            );
    }

    public void Exit(ICardDisplayStateContext context)
    {
    }
}
[Serializable]
public class IdlingDisplayState : ICardDisplayState
{
    public void Enter(ICardDisplayStateContext context)
    {
        context.SetPositionOffset(Vector2.zero);
        context.SetAngleOffset(Quaternion.identity);
    }

    public void Update(ICardDisplayStateContext context)
    {
        context.ApproachDestination();
    }

    public void Exit(ICardDisplayStateContext context)
    {
    }
}
[Serializable]
public class DraggingDisplayState : ICardDisplayState
{
    public void Enter(ICardDisplayStateContext context)
    {
        context.SizeUp();
    }

    public void Update(ICardDisplayStateContext context)
    {
        context.ApproachDestination();
    }

    public void Exit(ICardDisplayStateContext context)
    {
        context.SizeDown();
    }
}

public interface ICardDisplayState
{
    public void Enter(ICardDisplayStateContext context);
    public void Update(ICardDisplayStateContext context);
    public void Exit(ICardDisplayStateContext context);
}

public interface ICardDisplayStateContext
{
    public void ApproachDestination();
    public RectTransform RectTransform { get; }
    public Vector2 GetHoverLocation();
    public void SetAngleOffset(Quaternion angleOffset);
    public void SetPositionOffset(Vector2 position);
    public Vector3 InitialScale { get; }
    public Vector3 DesiredScale { get; set; }
    public void SizeUp();
    public void SizeDown();
}

public interface ICardDisplay : ILayerOrderable
{
    public void Target(Vector2 posTarget, float angleTarget);
    public void OnHover();
    public void OnHoverExit();
    public void OnStartDrag(float percent);
    public void OnEndDrag();
}

public interface ISizable
{
    public void Grow(float percent);
    public void Shrink();
    
}