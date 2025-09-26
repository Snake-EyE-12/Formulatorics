using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDisplay : MonoBehaviour, ICardDisplay
{
    private FollowTranslationalStrategy followTranslationalStrategy;
    private FollowRotationalStrategy followRotationalStrategy;

    [SerializeField] private CardDisplayEffectGimbal gimbal;
    [SerializeField] private CardDisplayEffectGimbal shadowGimbal;
    [SerializeField] private CardDisplayEffectShadow shadow;

    private Vector3 initialScale;
    private Vector3 desiredScale;

    private void Awake()
    {
        followTranslationalStrategy = GetComponent<FollowTranslationalStrategy>();
        followRotationalStrategy = GetComponent<FollowRotationalStrategy>();
        initialScale = transform.localScale;
        desiredScale = transform.localScale;
        float timeDifference = Random.value;
        gimbal.RandomizeTiming(timeDifference);
        shadowGimbal.RandomizeTiming(timeDifference);
    }

    
    
    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }
    public void SetSiblingOrder(int siblingIndex)
    {
        transform.SetSiblingIndex(siblingIndex);
    }

    private CardDisplayState displayState;

    private void AddState(CardDisplayState state)
    {
        displayState |= state;
        OnStateChange();
    }

    private void RemoveState(CardDisplayState state)
    {
        displayState &= ~state;
        OnStateChange();
    }

    private bool HasState(CardDisplayState state) => (displayState & state) != 0;

    private void OnStateChange()
    {
        if (HasState(CardDisplayState.Dragging))
        {
            shadow.SetYOffset();
            gimbal.FollowVelocity();
            shadowGimbal.FollowVelocity();
        }
        else
        {
            shadow.ResetYOffset();
            if (HasState(CardDisplayState.Hovered))
            {
                gimbal.FollowMouse();
                shadowGimbal.FollowMouse();
            }
            else
            {
                gimbal.Gyrate();
                shadowGimbal.Gyrate();
            }
        }
    }

    public void OnHoverEnter()
    {
        AddState(CardDisplayState.Hovered);
    }

    public void OnHoverExit()
    {
        RemoveState(CardDisplayState.Hovered);
    }

    public void OnDragStart()
    {
        AddState(CardDisplayState.Dragging);
    }

    public void OnDragStop()
    {
        RemoveState(CardDisplayState.Dragging);
    }


    private Vector2 anchorPos;
    private Quaternion anchorAngle;
    public void SetTargetAngle(float angle)
    {
        anchorAngle = Quaternion.Euler(0, 0, angle);
    }

    public void SetTargetLocation(Vector2 position)
    {
        anchorPos = position;
    }

    private Vector2 Target => anchorPos;
    private Quaternion TargetRotation => anchorAngle;
    public void ApproachDestination()
    {
        transform.position = followTranslationalStrategy.CalculatePosition(transform.position, Target);
        transform.rotation = followRotationalStrategy.CalculateRotation(transform.rotation, TargetRotation);
        transform.localScale = followTranslationalStrategy.CalculatePosition(transform.localScale, desiredScale);
    }
    

    public void AlterSize(float percent)
    {
        desiredScale = initialScale * percent;
    }

    public void ResetSize()
    {
        desiredScale = initialScale;
    }
}
[System.Flags]
public enum CardDisplayState
{
    None = 0,
    Hovered = 1 << 0,
    Dragging = 1 << 1,
}


public interface ICardDisplay : ILayerOrderable, IHasTargetDestination, ISizable
{
    public void OnHoverEnter();
    public void OnHoverExit();
    public void OnDragStart();
    public void OnDragStop();
}


public interface IHasTargetDestination
{
    public void SetTargetAngle(float angle);
    public void SetTargetLocation(Vector2 position);
    public void ApproachDestination();
}