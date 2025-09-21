using UnityEngine;

public class FollowStrategyRotationInstant : MonoBehaviour, FollowRotationalStrategy
{
    
    public Quaternion CalculateRotation(Quaternion current, Quaternion target)
    {
        return target;
    }
}